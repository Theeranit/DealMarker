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
    public partial class MA_TEMP_CTPY_LIMIT
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
            
            [Display(Name = "CTPY_LIMIT_ID")]
            [Required(ErrorMessage = "CTPY_LIMIT_ID is Required.")]	
            public  System.Guid CTPY_LIMIT_ID { get; set; }	
            
            [Display(Name = "AMOUNT")]
            [Required(ErrorMessage = "AMOUNT is Required.")]	
            public  decimal AMOUNT { get; set; }	
            
            [Display(Name = "EFFECTIVE_ DATE")]
            [Required(ErrorMessage = "EFFECTIVE_ DATE is Required.")]	
            public  System.DateTime EFFECTIVE_DATE { get; set; }	
            
            [Display(Name = "EXPIRY_ DATE")]
            [Required(ErrorMessage = "EXPIRY_ DATE is Required.")]	
            public  System.DateTime EXPIRY_DATE { get; set; }	
            
            [Display(Name = "ISACTIVE")]
            [Required(ErrorMessage = "ISACTIVE is Required.")]	
            public  bool ISACTIVE { get; set; }	

            #endregion
    
            #region Complex Properties
            
            [Display(Name = "LOG")]
            [Required(ErrorMessage = "LOG is Required.")]
            public  LOG LOG { get; set; }

            #endregion
    
    	}

        #endregion
    }
}
