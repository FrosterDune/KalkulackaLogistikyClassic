using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalkulackaLogistikyClassic
{
    internal class ZpracovaniTabulek
    {
        public class PrubeznaDobaTabulka
        {
            public int Tpz1 { get; set; }
            public int Tksum { get; set; }
            public int Tkmax { get; set; }
            public int Tmsum { get; set; }
            public int TmWithValue { get; set; }

            public PrubeznaDobaTabulka(DataGridView PruDobaDatatable, DataGridViewTextBoxColumn TkColumn, DataGridViewTextBoxColumn TmColumn)
            {
                Tpz1 = Convert.ToInt32(PruDobaDatatable.Rows[0].Cells[1].Value);
                Tksum = (from DataGridViewRow row in PruDobaDatatable.Rows select Convert.ToInt32(row.Cells[TkColumn.Index].Value.ToString())).Sum();
                Tkmax = (from DataGridViewRow row in PruDobaDatatable.Rows select Convert.ToInt32(row.Cells[TkColumn.Index].Value.ToString())).Max();
                Tmsum = (from DataGridViewRow row in PruDobaDatatable.Rows select Convert.ToInt32(row.Cells[TmColumn.Index].Value.ToString())).Sum();
                TmWithValue = (from DataGridViewRow row in PruDobaDatatable.Rows where Convert.ToInt32(row.Cells[TmColumn.Index].Value.ToString()) != 0 select row).Count();
            }
        }
    }
}