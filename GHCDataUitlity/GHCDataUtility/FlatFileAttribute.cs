using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ghc.Utility
{

    #region PUBLIC ENUM

    public enum Padding
    {
        Left,
        Right
    }

    #endregion

    #region PUBLIC CLASS: FlatFileAttribute

    public class FlatFileAttribute: Attribute
    {
        #region PUBLIC PROPERTIES
        
        public int Position { get; set; }
        public int Length { get; set; }
        public Padding Padding { get; set; }
        public string Format { get; set; }

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Initializes a new instance of the <see cref="FlatFileAttribute"/> class.
        /// </summary>
        /// <param name="position">Each item needs to be ordered so that 
        /// serialization/deserilization works even if the properties 
        /// are reordered in the class.</param>
        /// <param name="length">Total width in the text file</param>
        /// <param name="padding">How to do the padding</param>
        public FlatFileAttribute(int intPosition, int intLength, Padding ePadding, string strFormat)
        {
            this.Position = intPosition;
            this.Length = intLength;
            this.Padding = ePadding;
            this.Format = strFormat;
        }

        #endregion
    }

    #endregion
}