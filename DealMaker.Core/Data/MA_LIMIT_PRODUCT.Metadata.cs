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
    public partial class MA_LIMIT_PRODUCT
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
            
            [Display(Name = "PRODUCT_ID")]
            [Required(ErrorMessage = "PRODUCT_ID is Required.")]	
            public  System.Guid PRODUCT_ID { get; set; }	
            
            [Display(Name = "LIMIT_ID")]
            [Required(ErrorMessage = "LIMIT_ID is Required.")]	
            public  System.Guid LIMIT_ID { get; set; }	

            #endregion
    
    
            #region Navigation Properties
            	
            public  MA_PRODUCT MA_PRODUCT { get; set; }
            	
            public  MA_LIMIT MA_LIMIT { get; set; }

            #endregion
    	}

        #endregion
    }
}