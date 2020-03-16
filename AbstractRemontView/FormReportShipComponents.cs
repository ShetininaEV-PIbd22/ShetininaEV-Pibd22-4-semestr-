using AbstractRemontBusinessLogic.BusinessLogics;
using AbstractRemontBusinessLogic.BindingModels;
using System;
using System.Windows.Forms;
using Unity;

namespace AbstractRemontView
{
    public partial class FormReportShipComponents : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly ReportLogic logic;
        public FormReportShipComponents()
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void FormReportShipComponents_Load(object sender, EventArgs e)
        {

        }
        private void ButtonSaveToExcel_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        logic.SaveProductComponentToExcelFile(new ReportBindingModel
                        {
                            FileName = dialog.FileName
                        });
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
