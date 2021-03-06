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
    public partial class MA_COUTERPARTY
    {
        #region Metadata
    
    	/// <summary>
    	/// Metadata internal class used for Data Annotations
    	/// </summary>
    	internal class Metadata
        {
            #region Primitive Properties
            
            [Display(Name = "USERCODE")]
            [Required(ErrorMessage = "USERCODE is Required.")]	
            public  int USERCODE { get; set; }	
            
            [Display(Name = "SNAME")]
            [Required(ErrorMessage = "SNAME is Required.")]
            [StringLength(50, ErrorMessage = "SNAME must be under 50 characters.")]	
            public  string SNAME { get; set; }	
            
            [Display(Name = "FNAME")]
            [Required(ErrorMessage = "FNAME is Required.")]
            [StringLength(500, ErrorMessage = "FNAME must be under 500 characters.")]	
            public  string FNAME { get; set; }	
            
            [Display(Name = "BUSINESS")]
            [StringLength(100, ErrorMessage = "BUSINESS must be under 100 characters.")]	
            public  string BUSINESS { get; set; }	
            
            [Display(Name = "RATE")]
            [StringLength(10, ErrorMessage = "RATE must be under 10 characters.")]	
            public  string RATE { get; set; }	
            
            [Display(Name = "OUTLOOK")]
            [StringLength(50, ErrorMessage = "OUTLOOK must be under 50 characters.")]	
            public  string OUTLOOK { get; set; }	
            
            [Display(Name = "ISACTIVE")]	
            public  Nullable<bool> ISACTIVE { get; set; }	
            
            [Display(Name = "ID")]
            [Required(ErrorMessage = "ID is Required.")]	
            public  System.Guid ID { get; set; }	
            
            [Display(Name = "GROUP_CTPY_ID")]	
            public  Nullable<System.Guid> GROUP_CTPY_ID { get; set; }	
            
            [Display(Name = "TBMA_ NAME")]
            [StringLength(10, ErrorMessage = "TBMA_ NAME must be under 10 characters.")]	
            public  string TBMA_NAME { get; set; }	
            
            [Display(Name = "COUNTRY_ID")]
            [Required(ErrorMessage = "COUNTRY_ID is Required.")]	
            public  System.Guid COUNTRY_ID { get; set; }	

            #endregion
    
            #region Complex Properties
            
            [Display(Name = "LOG")]
            [Required(ErrorMessage = "LOG is Required.")]
            public  LOG LOG { get; set; }

            #endregion
    
            #region Navigation Properties
            	
            public  ICollection<MA_CTPY_LIMIT> MA_CTPY_LIMIT { get; set; }
            	
            public  MA_COUTERPARTY GROUP_CTPY { get; set; }
            	
            public  MA_COUNTRY MA_COUNTRY { get; set; }
            	
            public  MA_CSA_AGREEMENT MA_CSA_AGREEMENT { get; set; }

            #endregion
    	}

        #endregion
    }
}
