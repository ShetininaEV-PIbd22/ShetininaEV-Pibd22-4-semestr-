using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.BusinessLogics;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace AbstractRemontView
{
    public partial class FormAddSklad : Form
    {

        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly MainLogic mainLogic;
        private readonly ISkladLogic skladLogic;
        private readonly IComponentLogic componentLogic;
        public int Id
        {
            get { return Convert.ToInt32(comboBoxComponent.SelectedValue); }
            set { comboBoxComponent.SelectedValue = value; }
        }
        public string ComponentName { get { return comboBoxComponent.Text; } }

        public int Count
        {
            get { return Convert.ToInt32(textBoxCount.Text); }
            set { textBoxCount.Text = value.ToString(); }
        }

        public FormAddSklad(MainLogic logic, IComponentLogic componenmtLogic, ISkladLogic skladLogic)
        {
            InitializeComponent();
            this.mainLogic = logic;
            this.componentLogic = componenmtLogic;
            this.skladLogic = skladLogic;
            LoadData();
        }

        private void LoadData()
        {
            //  Получаем список изделий и заносим его в выпадающий список
            List<ComponentViewModel> list = componentLogic.Read(null);
            if (list != null)
            {
                comboBoxComponent.DisplayMember = "ComponentName";
                comboBoxComponent.ValueMember = "Id";
                comboBoxComponent.DataSource = list;
                comboBoxComponent.SelectedItem = null;
            }
            List<SkladViewModel> sklad = skladLogic.Read(null);
            if (sklad != null)
            {
                comboBoxSklad.DisplayMember = "SkladName";
                comboBoxSklad.ValueMember = "Id";
                comboBoxSklad.DataSource = sklad;
                comboBoxComponent.SelectedItem = null;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxSklad.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxComponent.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBoxCount.Text == string.Empty)
            {
                MessageBox.Show("Введите количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            mainLogic.AddComponents(new SkladComponentBindingModel()
            {
                SkladId= (comboBoxSklad.SelectedItem as SkladViewModel).Id,
                ComponentId =(comboBoxComponent.SelectedItem as ComponentViewModel).Id,
                Count = Convert.ToInt32(textBoxCount.Text)
            });
            MessageBox.Show("Пополнение склада прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
