using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace UnitOfWork
{
    public class CustomerU
    {
        private int _CustomerCode = 0;
        private List<AddressU> objAddresses = new List<AddressU>();
        private bool _IsDirty = false;
        private bool _IsNew = true;
        public int CustomerCode
        {
            get { return _CustomerCode; }
            set { _CustomerCode = value; }
        }
        private string _CustomerName = "";

        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }
        public void Add(AddressU obj)
        {
           
            objAddresses.Add(obj);
        }
        public void Committ()
        {
            DataAccess obj = new DataAccess();
            try
            {
                if (_IsNew)
                {
                    obj.InsertCustomer(_CustomerCode,
                                      _CustomerName);
                }
                else
                {
                    if (_IsDirty)
                    {
                        obj.UpdateCustomer(_CustomerCode,
                                           _CustomerName);
                    }
                }
                foreach (AddressU o in objAddresses)
                {
                    o.Committ();
                }
               
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                
            }

        }
        public AddressU getAddress(int i)
        {
            return objAddresses[i];
        }
        public void Load(int pCustomerCode)
        {
            DataAccess obj = new DataAccess();
            SqlDataReader oreader = obj.GetCustomer(pCustomerCode);
            while (oreader.Read())
            {
                _CustomerCode = pCustomerCode;
                _IsNew = false;
                _CustomerName = oreader["CustomerName"].ToString();
            }
            
            oreader = obj.GetAddress(CustomerCode);
          
            while (oreader.Read())
            {
                AddressU objadd = new AddressU();
               
                objadd.SetValue(CustomerCode,
                                oreader["Address"].ToString(),
                                oreader["Street"].ToString());
               
                Add(objadd);
            }
        }

    }
    public class AddressU
    {
        private int _CustomerCode = 0;
        private bool _IsDirty = false;
        private bool _IsNew = true;
        internal void SetValue(int CustomerCode, String AddressName, string Street)
        {
            _CustomerCode = CustomerCode;
            _Address = AddressName;
            _Street = Street;
            _IsNew = false;
        }
      
        public int CustomerCode
        {
            get { return _CustomerCode; }
            set 
            {
                _IsDirty = true;
                _CustomerCode = value; 
            }
        }
        private string _Address = "";

        public string AddressName
        {
            get { return _Address; }
            set 
            {
                _IsDirty = true;
                _Address = value; 
            }
        }
        private string _Street = "";

        public string Street
        {
            get { return _Street; }
            set 
            {
                _IsDirty = true;
                _Street = value; 
            }
        }
        
      
        public void Committ()
        {
            DataAccess obj = new DataAccess();
            if (_IsNew)
            {
                
                obj.InsertAddress(CustomerCode, 
                                  AddressName, 
                                  Street);
            }
            else
            {
                if (_IsDirty)
                {
                    obj.UpdateAddress(_CustomerCode,
                                    _Address,
                                    _Street);
                }
            }
        }
       
    }
}
