using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KalkulackaLogistikyClassic
{
    interface IMenuVolba
    {
        void ResetujVelikost();
        void VzdyNaObrazovce(ToolStripMenuItem toolStripMenu);
    }

    class MenuVolba : IMenuVolba
    {
        public Form _form;

        public MenuVolba(Form form)
        {
            _form = form;
        }
        public void ResetujVelikost()
        {
            _form.Size = _form.RestoreBounds.Size;
            var screen = Screen.FromControl(_form);
            _form.Left = screen.Bounds.Left + (screen.Bounds.Width - _form.Width) / 2;
            _form.Top = screen.Bounds.Top + (screen.Bounds.Height - _form.Height) / 2;
        }
        public void VzdyNaObrazovce(ToolStripMenuItem toolStripMenu)
        {
            if (toolStripMenu.CheckState == CheckState.Checked)
            {
                _form.TopMost = true;
            }
            else
            {
                _form.TopMost = false;
            }
        }
    }
}