namespace KalkulackaLogistikyClassic
{
    public partial class LogistikaForm : Form
    {
        //  Anal�za Z�sob Tab
        private float Spotreba;
        private float ObjednavaciDavka;
        private float Zp;
        private float Pokryti;
        private float Lhuta;
        private float TydnyRok;
        private decimal IntervalKontroly;
        private bool XzpChecked;
        private bool BQSystemRBChecked;
        private bool SQsystemRBChecked;
        //  Pr�b�n� Doba Tab
        private int Tpz1;
        private int Tksum;
        private int Tkmax;
        private int Tmsum;
        private int Varianta;
        private int PocetPracovist;
        private int TmWithValue;
        private decimal VyrobniDavka;
        private decimal DavkaQD;


        public LogistikaForm()
        {
            InitializeComponent();
            IMenuVolba menuVolba = new MenuVolba(this);
            ResetovatVelikostTSMI.Click += (s, e) => menuVolba.ResetujVelikost();
            VzdyVepreduTSMI.Click += (s, e) => menuVolba.VzdyNaObrazovce(VzdyVepreduTSMI);
        }

        private void LogistikaForm_Load(object sender, EventArgs e)
        {
            PruDobaDatatable.TopLeftHeaderCell.Value = "Pracovi�t�";
        }
        //  OPTIM�LN� D�VKA TAB
        private void DavkaVypocetButton_Click(object sender, EventArgs e)
        {
            try
            {
                int velikostPoptavkyInput = int.Parse(Dtextbox.Text);
                float NpzInput = float.Parse(NPZtextbox.Text);
                float NsInput = float.Parse(NStextbox.Text);
                float NjInput = float.Parse(NJtextbox.Text);
                float ObdobiInput = float.Parse(Ttextbox.Text);
                PocetniOperace.DavkaOperace davkaOperace = new();
                davkaOperace.DavkaVypocet(velikostPoptavkyInput, NpzInput, NsInput, NjInput, ObdobiInput);
                QoptVysledekTXT.Text = $"{davkaOperace}";
            }
            catch (FormatException)
            {
                MessageBox.Show("nespr�vn� form�t", "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //  ANALYZA Z�SOB TAB 
        private void ZasobaVypocetButton_Click(object sender, EventArgs e)
        {
            try
            {
                Spotreba = float.Parse(Spot�ebaTextbox.Text);
                ObjednavaciDavka = int.Parse(ObjDavkaTextbox.Text);
                if (XzpCheckBox.CheckState == CheckState.Checked) { Pokryti = float.Parse(PokrytiTextbox.Text); }
                if (XzpCheckBox.CheckState != CheckState.Checked) { Zp = int.Parse(ZpTextbox.Text); }
                Lhuta = float.Parse(DodaciLhutaTextbox.Text);
                TydnyRok = float.Parse(RokTydenTextbox.Text);
                IntervalKontroly = IntervalKontrolyNumeric.Value;
                XzpChecked = XzpCheckBox.Checked;
                BQSystemRBChecked = BQRadioButton.Checked;
                SQsystemRBChecked = SQRadioButton.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show("chyba: " + ex.Message, "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            PocetniOperace.ObjUrovneOperace objUrovne = new(Spotreba, ObjednavaciDavka, TydnyRok, Zp, Pokryti, Lhuta, IntervalKontroly);
            objUrovne.ObjUrovenVolba(XzpChecked, BQSystemRBChecked, SQsystemRBChecked);
            ObjUrovenVysledekTxt.Text = $"{objUrovne}";
        }
        // Kontroluje, jestli je Checkbox "ZP nen� �veden" za�krtnut a povoluje textbox pro v�po�et pojistn� z�soby
        private void XzpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (XzpCheckBox.CheckState == CheckState.Checked)
            {
                PokrytiTextbox.Enabled = true;
                ZpTextbox.Enabled = false;
                ZpTextbox.Text = string.Empty;
            }
            else
            {
                ZpTextbox.Enabled = true;
                PokrytiTextbox.Enabled = false;
                PokrytiTextbox.Text = string.Empty;
            }
        }
        // P�ev�d� po�et dn� na t�dny v textboxu pro zad�n� �asov�ho horizontu
        private void PrevodButton_Click(object sender, EventArgs e)
        {
            try
            {
                float PrevodNaTydny = float.Parse(RokTydenTextbox.Text) / 7;
                RokTydenTextbox.Text = PrevodNaTydny.ToString();
            }
            catch (FormatException)
            {
                MessageBox.Show("Zadejte prvn� po�et dn�", "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Povoluje Numerick� kontrolej pro zad�n� interval kontroly, kter� je pro s,Q syst�m pot�eba zadat
        private void SQRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (SQRadioButton.Checked)
            {
                IntervalKontrolyNumeric.Enabled = true;
            }
            else
            {
                IntervalKontrolyNumeric.Enabled = false;
            }
        }
        
        //  PR�B̎N� DOBA TAB
        private void PruDobaVypocetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ZpracovaniTabulek.PrubeznaDobaTabulka prubeznaDobaZpracovani = new(PruDobaDatatable, TkColumn, TmColumn);
                Tpz1 = prubeznaDobaZpracovani.Tpz1;
                Tksum = prubeznaDobaZpracovani.Tksum;
                Tkmax = prubeznaDobaZpracovani.Tkmax;
                Tmsum = prubeznaDobaZpracovani.Tmsum;
                TmWithValue = prubeznaDobaZpracovani.TmWithValue;
                VyrobniDavka = (int)QdavkaNumeric.Value;
                Varianta = VariantyCmb.SelectedIndex;
                DavkaQD = (int)QdDavkaNumeric.Value;
                PocetPracovist = PruDobaDatatable.Rows.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show("chyba: " + ex.Message, "chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            PocetniOperace.PrubeznaDobaOperace prubeznaDoba = new(Tpz1, Tksum, Tkmax, Tmsum, 
                (int)VyrobniDavka, Varianta, (int)DavkaQD, PocetPracovist, TmWithValue);
            prubeznaDoba.ZvolenaVarianta(Varianta);
            PrubDobaVypisTxt.Text = $"{prubeznaDoba}";
        }
        // P�id� ��dek do datatable, p�esn�ji pracovi�t�
        private void AddrowBtn_Click(object sender, EventArgs e)
        {
            PruDobaDatatable.Rows.Add();
            SetPracovisteNumber(PruDobaDatatable);
        }
        // Sma�e posledn� vytvo�en� pracovi�t� a z�rove� zkontroluje, jestli se n�jak� ��dek v�bec v datatable nach�z�
        private void DelrowBtn_Click(object sender, EventArgs e)
        {
            if (PruDobaDatatable.Rows.Count > 0)
            {
                PruDobaDatatable.Rows.RemoveAt(PruDobaDatatable.Rows.Count - 1);
            }
        }
        // Automaticky p�id�luje ��slo pracovi�t� 
        private static void SetPracovisteNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = string.Format($"{row.Index + 1}");
            }
        }
        //  Kontroluje, jak� metoda je vybr�na, a podle toho aktivuje �i deaktivuje QdDavkaNumeric kter� je pro variantu po d�vk�ch d�le�it�
        private void VariantyCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (VariantyCmb.SelectedIndex == 1) { QdDavkaNumeric.Enabled = true; }
            else { QdDavkaNumeric.Enabled = false; }
        }
    }
}