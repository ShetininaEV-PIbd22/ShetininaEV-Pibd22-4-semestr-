using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;

namespace AbstractRemontView
{
    public partial class FormShip : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IShipLogic logic;

        private int? id;

        private Dictionary<int, (string, int)> productIngredients;

        public FormShip(IShipLogic service)
        {
            InitializeComponent();
            logic = service;
        }

        private void FormShip_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ShipViewModel view = logic.Read(new ShipBindingModel { Id = id.Value })?[0];
                    if (view != null)
                    {
                        textBoxName.Text = view.ShipName;
                        textBoxPrice.Text = view.Price.ToString();
                        productIngredients = view.ShipComponents;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                productIngredients = new Dictionary<int, (string, int)>();
        }

        private void LoadData()
        {
            try
            {
                dataGridView.Columns.Clear();
                dataGridView.Columns.Add("Number", "№");
                dataGridView.Columns.Add("Ingredients", "Ингредиенты");
                dataGridView.Columns.Add("Count", "Количество");
                if (productIngredients != null)
                {
                    dataGridView.Rows.Clear();
                    foreach (var pi in productIngredients)
                        dataGridView.Rows.Add(new object[] { pi.Key, pi.Value.Item1, pi.Value.Item2 });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormShipComponent>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (productIngredients.ContainsKey(form.Id))
                {
                    productIngredients[form.Id] = (form.ComponentName, form.Count);
                }
                else
                {
                    productIngredients.Add(form.Id, (form.ComponentName, form.Count));
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormShipComponent>();
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                form.Id = id;
                form.Count = productIngredients[id].Item2;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    productIngredients[form.Id] = (form.ComponentName, form.Count);
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        productIngredients.Remove(Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (productIngredients == null || productIngredients.Count == 0)
            {
                MessageBox.Show("Заполните ингредиенты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                foreach (var pi in productIngredients)
                {
                    Console.WriteLine(pi);
                }
                logic.CreateOrUpdate(new ShipBindingModel
                {
                    Id = id,
                    ShipName = textBoxName.Text,
                    Price = Convert.ToDecimal(textBoxPrice.Text),
                    ShipComponents = productIngredients
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
