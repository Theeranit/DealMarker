using System;
using System.Data.Entity;

namespace KK.DealMaker.DataAccess.Repositories
{
	public class EFUnitOfWork : IUnitOfWork, IDisposable
	{
		private DbContext _context;

		public EFUnitOfWork()
		{
			_context = new KKLMDBEntities();
		}

		public void Commit()
		{
			_context.SaveChanges();
		}
		
		public bool LazyLoadingEnabled
		{
			get { return _context.Configuration.LazyLoadingEnabled; }
			set { _context.Configuration.LazyLoadingEnabled = value; }
		}

		public bool ProxyCreationEnabled
		{
			get { return _context.Configuration.ProxyCreationEnabled; }
			set { _context.Configuration.ProxyCreationEnabled = value; }
		}
		
		public string ConnectionString
		{
			get { return _context.Database.Connection.ConnectionString; }
			set { _context.Database.Connection.ConnectionString = value; }
		}
		private DA_LOGGINGRepository _DA_LOGGINGRepository;
        public DA_LOGGINGRepository DA_LOGGINGRepository
        {
            get
            {

                if (this._DA_LOGGINGRepository == null)
                {
                    this._DA_LOGGINGRepository = new DA_LOGGINGRepository(_context);
                }
                return _DA_LOGGINGRepository;
            }
        }
		private DA_LOGIN_AUDITRepository _DA_LOGIN_AUDITRepository;
        public DA_LOGIN_AUDITRepository DA_LOGIN_AUDITRepository
        {
            get
            {

                if (this._DA_LOGIN_AUDITRepository == null)
                {
                    this._DA_LOGIN_AUDITRepository = new DA_LOGIN_AUDITRepository(_context);
                }
                return _DA_LOGIN_AUDITRepository;
            }
        }
		private DA_TMBA_EXTENSIONRepository _DA_TMBA_EXTENSIONRepository;
        public DA_TMBA_EXTENSIONRepository DA_TMBA_EXTENSIONRepository
        {
            get
            {

                if (this._DA_TMBA_EXTENSIONRepository == null)
                {
                    this._DA_TMBA_EXTENSIONRepository = new DA_TMBA_EXTENSIONRepository(_context);
                }
                return _DA_TMBA_EXTENSIONRepository;
            }
        }
		private DA_TRNRepository _DA_TRNRepository;
        public DA_TRNRepository DA_TRNRepository
        {
            get
            {

                if (this._DA_TRNRepository == null)
                {
                    this._DA_TRNRepository = new DA_TRNRepository(_context);
                }
                return _DA_TRNRepository;
            }
        }
		private DA_TRN_CASHFLOWRepository _DA_TRN_CASHFLOWRepository;
        public DA_TRN_CASHFLOWRepository DA_TRN_CASHFLOWRepository
        {
            get
            {

                if (this._DA_TRN_CASHFLOWRepository == null)
                {
                    this._DA_TRN_CASHFLOWRepository = new DA_TRN_CASHFLOWRepository(_context);
                }
                return _DA_TRN_CASHFLOWRepository;
            }
        }
		private DA_TRN_MATCHRepository _DA_TRN_MATCHRepository;
        public DA_TRN_MATCHRepository DA_TRN_MATCHRepository
        {
            get
            {

                if (this._DA_TRN_MATCHRepository == null)
                {
                    this._DA_TRN_MATCHRepository = new DA_TRN_MATCHRepository(_context);
                }
                return _DA_TRN_MATCHRepository;
            }
        }
		private MA_BOND_MARKETRepository _MA_BOND_MARKETRepository;
        public MA_BOND_MARKETRepository MA_BOND_MARKETRepository
        {
            get
            {

                if (this._MA_BOND_MARKETRepository == null)
                {
                    this._MA_BOND_MARKETRepository = new MA_BOND_MARKETRepository(_context);
                }
                return _MA_BOND_MARKETRepository;
            }
        }
		private MA_CONFIG_ATTRIBUTERepository _MA_CONFIG_ATTRIBUTERepository;
        public MA_CONFIG_ATTRIBUTERepository MA_CONFIG_ATTRIBUTERepository
        {
            get
            {

                if (this._MA_CONFIG_ATTRIBUTERepository == null)
                {
                    this._MA_CONFIG_ATTRIBUTERepository = new MA_CONFIG_ATTRIBUTERepository(_context);
                }
                return _MA_CONFIG_ATTRIBUTERepository;
            }
        }
		private MA_COUNTRYRepository _MA_COUNTRYRepository;
        public MA_COUNTRYRepository MA_COUNTRYRepository
        {
            get
            {

                if (this._MA_COUNTRYRepository == null)
                {
                    this._MA_COUNTRYRepository = new MA_COUNTRYRepository(_context);
                }
                return _MA_COUNTRYRepository;
            }
        }
		private MA_COUNTRY_LIMITRepository _MA_COUNTRY_LIMITRepository;
        public MA_COUNTRY_LIMITRepository MA_COUNTRY_LIMITRepository
        {
            get
            {

                if (this._MA_COUNTRY_LIMITRepository == null)
                {
                    this._MA_COUNTRY_LIMITRepository = new MA_COUNTRY_LIMITRepository(_context);
                }
                return _MA_COUNTRY_LIMITRepository;
            }
        }
		private MA_COUTERPARTYRepository _MA_COUTERPARTYRepository;
        public MA_COUTERPARTYRepository MA_COUTERPARTYRepository
        {
            get
            {

                if (this._MA_COUTERPARTYRepository == null)
                {
                    this._MA_COUTERPARTYRepository = new MA_COUTERPARTYRepository(_context);
                }
                return _MA_COUTERPARTYRepository;
            }
        }
		private MA_CSA_AGREEMENTRepository _MA_CSA_AGREEMENTRepository;
        public MA_CSA_AGREEMENTRepository MA_CSA_AGREEMENTRepository
        {
            get
            {

                if (this._MA_CSA_AGREEMENTRepository == null)
                {
                    this._MA_CSA_AGREEMENTRepository = new MA_CSA_AGREEMENTRepository(_context);
                }
                return _MA_CSA_AGREEMENTRepository;
            }
        }
		private MA_CSA_PRODUCTRepository _MA_CSA_PRODUCTRepository;
        public MA_CSA_PRODUCTRepository MA_CSA_PRODUCTRepository
        {
            get
            {

                if (this._MA_CSA_PRODUCTRepository == null)
                {
                    this._MA_CSA_PRODUCTRepository = new MA_CSA_PRODUCTRepository(_context);
                }
                return _MA_CSA_PRODUCTRepository;
            }
        }
		private MA_CSA_TYPERepository _MA_CSA_TYPERepository;
        public MA_CSA_TYPERepository MA_CSA_TYPERepository
        {
            get
            {

                if (this._MA_CSA_TYPERepository == null)
                {
                    this._MA_CSA_TYPERepository = new MA_CSA_TYPERepository(_context);
                }
                return _MA_CSA_TYPERepository;
            }
        }
		private MA_CTPY_LIMITRepository _MA_CTPY_LIMITRepository;
        public MA_CTPY_LIMITRepository MA_CTPY_LIMITRepository
        {
            get
            {

                if (this._MA_CTPY_LIMITRepository == null)
                {
                    this._MA_CTPY_LIMITRepository = new MA_CTPY_LIMITRepository(_context);
                }
                return _MA_CTPY_LIMITRepository;
            }
        }
		private MA_CURRENCYRepository _MA_CURRENCYRepository;
        public MA_CURRENCYRepository MA_CURRENCYRepository
        {
            get
            {

                if (this._MA_CURRENCYRepository == null)
                {
                    this._MA_CURRENCYRepository = new MA_CURRENCYRepository(_context);
                }
                return _MA_CURRENCYRepository;
            }
        }
		private MA_FREQ_TYPERepository _MA_FREQ_TYPERepository;
        public MA_FREQ_TYPERepository MA_FREQ_TYPERepository
        {
            get
            {

                if (this._MA_FREQ_TYPERepository == null)
                {
                    this._MA_FREQ_TYPERepository = new MA_FREQ_TYPERepository(_context);
                }
                return _MA_FREQ_TYPERepository;
            }
        }
		private MA_FUNCTIONALRepository _MA_FUNCTIONALRepository;
        public MA_FUNCTIONALRepository MA_FUNCTIONALRepository
        {
            get
            {

                if (this._MA_FUNCTIONALRepository == null)
                {
                    this._MA_FUNCTIONALRepository = new MA_FUNCTIONALRepository(_context);
                }
                return _MA_FUNCTIONALRepository;
            }
        }
		private MA_INSTRUMENTRepository _MA_INSTRUMENTRepository;
        public MA_INSTRUMENTRepository MA_INSTRUMENTRepository
        {
            get
            {

                if (this._MA_INSTRUMENTRepository == null)
                {
                    this._MA_INSTRUMENTRepository = new MA_INSTRUMENTRepository(_context);
                }
                return _MA_INSTRUMENTRepository;
            }
        }
		private MA_LIMITRepository _MA_LIMITRepository;
        public MA_LIMITRepository MA_LIMITRepository
        {
            get
            {

                if (this._MA_LIMITRepository == null)
                {
                    this._MA_LIMITRepository = new MA_LIMITRepository(_context);
                }
                return _MA_LIMITRepository;
            }
        }
		private MA_LIMIT_PRODUCTRepository _MA_LIMIT_PRODUCTRepository;
        public MA_LIMIT_PRODUCTRepository MA_LIMIT_PRODUCTRepository
        {
            get
            {

                if (this._MA_LIMIT_PRODUCTRepository == null)
                {
                    this._MA_LIMIT_PRODUCTRepository = new MA_LIMIT_PRODUCTRepository(_context);
                }
                return _MA_LIMIT_PRODUCTRepository;
            }
        }
		private MA_PCCFRepository _MA_PCCFRepository;
        public MA_PCCFRepository MA_PCCFRepository
        {
            get
            {

                if (this._MA_PCCFRepository == null)
                {
                    this._MA_PCCFRepository = new MA_PCCFRepository(_context);
                }
                return _MA_PCCFRepository;
            }
        }
		private MA_PCCF_CONFIGRepository _MA_PCCF_CONFIGRepository;
        public MA_PCCF_CONFIGRepository MA_PCCF_CONFIGRepository
        {
            get
            {

                if (this._MA_PCCF_CONFIGRepository == null)
                {
                    this._MA_PCCF_CONFIGRepository = new MA_PCCF_CONFIGRepository(_context);
                }
                return _MA_PCCF_CONFIGRepository;
            }
        }
		private MA_PORTFOLIORepository _MA_PORTFOLIORepository;
        public MA_PORTFOLIORepository MA_PORTFOLIORepository
        {
            get
            {

                if (this._MA_PORTFOLIORepository == null)
                {
                    this._MA_PORTFOLIORepository = new MA_PORTFOLIORepository(_context);
                }
                return _MA_PORTFOLIORepository;
            }
        }
		private MA_PROCESS_DATERepository _MA_PROCESS_DATERepository;
        public MA_PROCESS_DATERepository MA_PROCESS_DATERepository
        {
            get
            {

                if (this._MA_PROCESS_DATERepository == null)
                {
                    this._MA_PROCESS_DATERepository = new MA_PROCESS_DATERepository(_context);
                }
                return _MA_PROCESS_DATERepository;
            }
        }
		private MA_PRODUCTRepository _MA_PRODUCTRepository;
        public MA_PRODUCTRepository MA_PRODUCTRepository
        {
            get
            {

                if (this._MA_PRODUCTRepository == null)
                {
                    this._MA_PRODUCTRepository = new MA_PRODUCTRepository(_context);
                }
                return _MA_PRODUCTRepository;
            }
        }
		private MA_PROFILE_FUNCTIONALRepository _MA_PROFILE_FUNCTIONALRepository;
        public MA_PROFILE_FUNCTIONALRepository MA_PROFILE_FUNCTIONALRepository
        {
            get
            {

                if (this._MA_PROFILE_FUNCTIONALRepository == null)
                {
                    this._MA_PROFILE_FUNCTIONALRepository = new MA_PROFILE_FUNCTIONALRepository(_context);
                }
                return _MA_PROFILE_FUNCTIONALRepository;
            }
        }
		private MA_SPOT_RATERepository _MA_SPOT_RATERepository;
        public MA_SPOT_RATERepository MA_SPOT_RATERepository
        {
            get
            {

                if (this._MA_SPOT_RATERepository == null)
                {
                    this._MA_SPOT_RATERepository = new MA_SPOT_RATERepository(_context);
                }
                return _MA_SPOT_RATERepository;
            }
        }
		private MA_STATUSRepository _MA_STATUSRepository;
        public MA_STATUSRepository MA_STATUSRepository
        {
            get
            {

                if (this._MA_STATUSRepository == null)
                {
                    this._MA_STATUSRepository = new MA_STATUSRepository(_context);
                }
                return _MA_STATUSRepository;
            }
        }
		private MA_TBMA_CONFIGRepository _MA_TBMA_CONFIGRepository;
        public MA_TBMA_CONFIGRepository MA_TBMA_CONFIGRepository
        {
            get
            {

                if (this._MA_TBMA_CONFIGRepository == null)
                {
                    this._MA_TBMA_CONFIGRepository = new MA_TBMA_CONFIGRepository(_context);
                }
                return _MA_TBMA_CONFIGRepository;
            }
        }
		private MA_TEMP_CTPY_LIMITRepository _MA_TEMP_CTPY_LIMITRepository;
        public MA_TEMP_CTPY_LIMITRepository MA_TEMP_CTPY_LIMITRepository
        {
            get
            {

                if (this._MA_TEMP_CTPY_LIMITRepository == null)
                {
                    this._MA_TEMP_CTPY_LIMITRepository = new MA_TEMP_CTPY_LIMITRepository(_context);
                }
                return _MA_TEMP_CTPY_LIMITRepository;
            }
        }
		private MA_USERRepository _MA_USERRepository;
        public MA_USERRepository MA_USERRepository
        {
            get
            {

                if (this._MA_USERRepository == null)
                {
                    this._MA_USERRepository = new MA_USERRepository(_context);
                }
                return _MA_USERRepository;
            }
        }
		private MA_USER_PROFILERepository _MA_USER_PROFILERepository;
        public MA_USER_PROFILERepository MA_USER_PROFILERepository
        {
            get
            {

                if (this._MA_USER_PROFILERepository == null)
                {
                    this._MA_USER_PROFILERepository = new MA_USER_PROFILERepository(_context);
                }
                return _MA_USER_PROFILERepository;
            }
        }


		private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
	}
}
