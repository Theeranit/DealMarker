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
    public partial class MA_COUNTRY_LIMIT
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
            
            [Display(Name = "COUNTRY_ID")]
            [Required(ErrorMessage = "COUNTRY_ID is Required.")]	
            public  System.Guid COUNTRY_ID { get; set; }	
            
            [Display(Name = "AMOUNT")]
            [Required(ErrorMessage = "AMOUNT is Required.")]	
            public  decimal AMOUNT { get; set; }	
            
            [Display(Name = "EFFECTIVE_ DATE")]
            [Required(ErrorMessage = "EFFECTIVE_ DATE is Required.")]	
            public  System.DateTime EFFECTIVE_DATE { get; set; }	
            
            [Display(Name = "EXPIRY_ DATE")]
            [Required(ErrorMessage = "EXPIRY_ DATE is Required.")]	
            public  System.DateTime EXPIRY_DATE { get; set; }	
            
            [Display(Name = "ISTEMP")]
            [Required(ErrorMessage = "ISTEMP is Required.")]	
            public  bool ISTEMP { get; set; }	
            
            [Display(Name = "ISACTIVE")]
            [Required(ErrorMessage = "ISACTIVE is Required.")]	
            public  bool ISACTIVE { get; set; }	
            
            [Display(Name = "FLAG_ CONTROL")]
            [Required(ErrorMessage = "FLAG_ CONTROL is Required.")]	
            public  bool FLAG_CONTROL { get; set; }	

            #endregion
    
            #region Complex Properties
            
            [Display(Name = "LOG")]
            [Required(ErrorMessage = "LOG is Required.")]
            public  LOG LOG { get; set; }

            #endregion
    
            #region Navigation Properties
            	
            public  MA_COUNTRY MA_COUNTRY { get; set; }

            #endregion
    	}

        #endregion
    }
}
