//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KK.DealMaker.Core.Data
{
    [MetadataType(typeof(Metadata))]
    public partial class MA_SPOT_RATE
    {
        #region Metadata
    
    	/// <summary>
    	/// Metadata internal class used for Data Annotations
    	/// </summary>
    	internal class Metadata
        {
            #region Primitive Properties
            
            [Display(Name = "ID")]
            [Required(ErrorMessage = "ID is Required.")]	
            public  System.Guid ID { get; set; }	
            
            [Display(Name = "CURRENCY_ID")]
            [Required(ErrorMessage = "CURRENCY_ID is Required.")]	
            public  System.Guid CURRENCY_ID { get; set; }	
            
            [Display(Name = "PROC_ DATE")]
            [Required(ErrorMessage = "PROC_ DATE is Required.")]	
            public  System.DateTime PROC_DATE { get; set; }	
            
            [Display(Name = "RATE")]
            [Required(ErrorMessage = "RATE is Required.")]	
            public  decimal RATE { get; set; }	

            #endregion
    
    
            #region Navigation Properties
            	
            public  MA_CURRENCY MA_CURRENCY { get; set; }

            #endregion
    	}

        #endregion
    }
}