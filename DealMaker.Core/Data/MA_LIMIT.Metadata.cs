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
    public partial class MA_LIMIT
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
            
            [Display(Name = "LABEL")]
            [Required(ErrorMessage = "LABEL is Required.")]
            [StringLength(50, ErrorMessage = "LABEL must be under 50 characters.")]	
            public  string LABEL { get; set; }	
            
            [Display(Name = "ISACTIVE")]
            [Required(ErrorMessage = "ISACTIVE is Required.")]	
            public  bool ISACTIVE { get; set; }	
            
            [Display(Name = "LIMIT_ TYPE")]
            [Required(ErrorMessage = "LIMIT_ TYPE is Required.")]
            [StringLength(50, ErrorMessage = "LIMIT_ TYPE must be under 50 characters.")]	
            public  string LIMIT_TYPE { get; set; }	
            
            [Display(Name = "INDEX")]	
            public  Nullable<int> INDEX { get; set; }	

            #endregion
    
    
            #region Navigation Properties
            	
            public  ICollection<MA_LIMIT_PRODUCT> MA_LIMIT_PRODUCT { get; set; }

            #endregion
    	}

        #endregion
    }
}