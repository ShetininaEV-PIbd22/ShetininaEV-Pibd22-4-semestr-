using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.BusinessLogics;
using AbstractRemontBusinessLogic.Interfaces;
using System;
using System.Windows.Forms;
using Unity;

namespace AbstractRemontView
{
    public partial class FormMain : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly MainLogic logic;
        private readonly WorkModeling work;
        private readonly IRemontLogic orderLogic;
        private readonly ReportLogic report;
        private readonly BackUpAbstractLogic backUpAbstractLogic;

        public FormMain(MainLogic logic, ReportLogic report, WorkModeling work, IRemontLogic orderLogic,
            BackUpAbstractLogic backUpAbstractLogic)
        {
            InitializeComponent();
            this.logic = logic;
            this.orderLogic = orderLogic;
            this.work = work;
            this.report = report;
            this.backUpAbstractLogic = backUpAbstractLogic;
        }

        private void компонентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormComponents>();
            form.ShowDialog();
        }

        private void изделияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormShips>();
            form.ShowDialog();
        }
        private void исполнителиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormImplementers>();
            form.ShowDialog();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                Program.ConfigGrid(orderLogic.Read(null), dataGridView);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void buttonCreateRemont_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCreateRemont>();
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void buttonTakeRemontInWork_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    logic.TakeRemontInWork(new ChangeStatusBindingModel { RemontId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonRemontReady_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    logic.FinishRemont(new ChangeStatusBindingModel { RemontId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonPayRemont_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                try
                {
                    logic.PayRemont(new ChangeStatusBindingModel { RemontId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void списокКораблейToolStripMenuItem_Click(object sender, EventArgs e)
        {
             using (var dialog = new SaveFileDialog { Filter = "docx|*.docx" })
             {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    report.SaveProductsToWordFile(new ReportBindingModel { FileName = dialog.FileName });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
             }
        }

        private void компонентыПоКораблямToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportShipComponents>();
            form.ShowDialog();
        }

        private void списокЗаказовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportRemonts>();
            form.ShowDialog();
        }

        private void клиентыToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormClients>();
            form.ShowDialog();
        }

        private void buttonStartWork_Click(object sender, EventArgs e)
        {
            work.DoWork();
            LoadData();
        }

        private void buttonMessages_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormMessages>();
            form.ShowDialog();
        }

        private void создатьБекапToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (backUpAbstractLogic != null)
                {
                    var fbd = new FolderBrowserDialog();
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        backUpAbstractLogic.CreateArchive(fbd.SelectedPath);
                        MessageBox.Show("Бекап создан", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
