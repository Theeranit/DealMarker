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
    public partial class MA_PRODUCT
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
            [StringLength(50, ErrorMessage = "LABEL must be under 50 characters.")]	
            public  string LABEL { get; set; }	
            
            [Display(Name = "ISACTIVE")]	
            public  Nullable<bool> ISACTIVE { get; set; }	

            #endregion
    
    
            #region Navigation Properties
            	
            public  ICollection<MA_LIMIT_PRODUCT> MA_LIMIT_PRODUCT { get; set; }
            	
            private ICollection<MA_INSTRUMENT> MA_INSRUMENT { get; set; }
            	
            public  ICollection<MA_PCCF_CONFIG> MA_PCCF_CONFIG { get; set; }

            #endregion
    	}

        #endregion
    }
}
