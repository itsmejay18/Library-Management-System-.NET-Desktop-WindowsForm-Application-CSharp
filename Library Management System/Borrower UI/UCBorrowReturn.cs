using Library_Management_System.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System.Borrower_UI
{
    public partial class UCBorrowReturn : UserControl
    {
        public UCBorrowReturn()
        {
            InitializeComponent();
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            BorrowBook borrowForm = new BorrowBook();
            borrowForm.ShowDialog(); // Opens the borrow form
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            ReturnBook returnForm = new ReturnBook();
            returnForm.ShowDialog(); // Opens the return form
        }
    }
}
