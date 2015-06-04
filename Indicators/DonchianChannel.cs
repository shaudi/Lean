﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using QuantConnect.Data.Market;

namespace QuantConnect.Indicators
{
    /// <summary>
    /// This indicator computes the upper and lower band of the Donchian Channel.
    /// The upper band is computed by finding the highest high over the given period.
    /// The lower band is computed by finding the lowest low over the given period.
    /// </summary>
    public class DonchianChannel : TradeBarIndicator
    {
        private TradeBar _previousInput;
        /// <summary>
        /// Gets the upper band of the Donchian Channel.
        /// </summary>
        public IndicatorBase<IndicatorDataPoint> UpperBand { get; private set; }

        /// <summary>
        /// Gets the lower band of the Donchian Channel.
        /// </summary>
        public IndicatorBase<IndicatorDataPoint> LowerBand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DonchianChannel"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="period">The period.</param>
        public DonchianChannel(string name, int period)
            : base(name)
        {
            UpperBand = new Maximum(name + "_UpperBand", period);
            LowerBand = new Minimum(name + "_LowerBand", period);
        }

        /// <summary>
        /// Gets a flag indicating when this indicator is ready and fully initialized
        /// </summary>
        public override bool IsReady
        {
            get { return UpperBand.IsReady && LowerBand.IsReady; }
        }

        /// <summary>
        /// Computes the next value of this indicator from the given state
        /// </summary>
        /// <param name="input">The input given to the indicator</param>
        /// <returns>A new value for this indicator, which by convention is the middle band of the Donchian Channel if called upon.</returns>
        protected override decimal ComputeNextValue(TradeBar input)
        {
            if (_previousInput != null)
            {
                UpperBand.Update(new IndicatorDataPoint(_previousInput.Time, _previousInput.High));
                LowerBand.Update(new IndicatorDataPoint(_previousInput.Time, _previousInput.Low));
            }

            _previousInput = input;
            return UpperBand.Current.Value + LowerBand.Current.Value / 2;
        }

    }
}
