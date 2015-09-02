using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KK.DealMaker.Business.Master;
using KK.DealMaker.Core.Data;

namespace DealMaker.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            PCCFConfigBusiness pccfBusiness = new PCCFConfigBusiness();

            DA_TRN record = new DA_TRN();
            record.ID = Guid.NewGuid();
            record.ENGINE_DATE = DateTime.Now;
            record.STATUS_ID = new Guid("9161ED18-1298-44FA-BA7D-34522CB40D66");
            record.INSTRUMENT_ID = new Guid("33F88EB3-46C0-4667-BCF6-05426321B71B");
            record.PRODUCT_ID = new Guid("F85252D1-BC58-4AC6-8B56-2E228FB0A367");
            record.LOG.INSERTBYUSERID = new Guid("F85252D1-BC58-4AC6-8B56-2E228FB0A367");
            record.LOG.INSERTDATE = DateTime.Now;

            record.FIRST.CCY_ID = new Guid("825F343B-CAEA-409B-AE92-CCA2DAB3765E");
            record.FIRST.FLAG_PAYREC = "R";
            record.FIRST.FLAG_FIXED = false;
            record.SECOND.CCY_ID = new Guid("825F343B-CAEA-409B-AE92-CCA2DAB3765E");
            record.SECOND.FLAG_PAYREC = "P";
            record.SECOND.FLAG_FIXED = false ;
            
            var temp = pccfBusiness.ValidatePCCFConfig(null, record);
            if(temp!=null)
                Console.WriteLine("PCCF:" + temp.LABEL);
            else
                Console.WriteLine("PCCF is not match");
            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
