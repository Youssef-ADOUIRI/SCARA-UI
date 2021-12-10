using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCARA_UI.MVVM.Model
{
    class BackEnd
    {

        private int _comNo;
        public int comNo
        {
            get
            {
                return _comNo;
            }
            set
            {
                _comNo = value;
            }
        }

    }
}
