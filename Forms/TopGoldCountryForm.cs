using Olimpiada.DataAccess;
using Olimpiada.Models;

namespace Olimpiada.Forms
{
    public partial class TopGoldCountryForm : Form
    {
        private DatabaseHelper dbHelper;
        private bool byOlympics;
        private Label lblResult;
        private ComboBox cmbOlympics;
        private Label lblOlympics;
        private Button btnShow;
        private Button btnClose;

        public TopGoldCountryForm(DatabaseHelper dbHelper, bool byOlympics)
        {
            this.dbHelper = dbHelper;
            this.byOlympics = byOlympics;
            InitializeComponent();
            if (byOlympics)
                LoadOlympics();
            else
                LoadData();
        }

        private void InitializeComponent()
        {
            lblResult = new Label();
            cmbOlympics = new ComboBox();
            lblOlympics = new Label();
            btnShow = new Button();
            btnClose = new Button();
            SuspendLayout();

            lblOlympics.AutoSize = true;
            lblOlympics.Location = new Point(12, 15);
            lblOlympics.Name = "lblOlympics";
            lblOlympics.Size = new Size(70, 15);
            lblOlympics.Text = "Олимпиада:";
            lblOlympics.Visible = byOlympics;

            cmbOlympics.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOlympics.FormattingEnabled = true;
            cmbOlympics.Location = new Point(100, 12);
            cmbOlympics.Name = "cmbOlympics";
            cmbOlympics.Size = new Size(300, 23);
            cmbOlympics.TabIndex = 0;
            cmbOlympics.Visible = byOlympics;

            btnShow.Location = new Point(406, 12);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(100, 30);
            btnShow.TabIndex = 1;
            btnShow.Text = "Показать";
            btnShow.UseVisualStyleBackColor = true;
            btnShow.Visible = byOlympics;
            btnShow.Click += BtnShow_Click;

            lblResult.AutoSize = false;
            lblResult.Location = new Point(12, byOlympics ? 50 : 12);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(560, 200);
            lblResult.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lblResult.TextAlign = ContentAlignment.MiddleCenter;

            btnClose.Location = new Point(472, byOlympics ? 260 : 222);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(100, 30);
            btnClose.TabIndex = 2;
            btnClose.Text = "Закрыть";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += BtnClose_Click;

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, byOlympics ? 302 : 264);
            Controls.Add(btnClose);
            Controls.Add(lblResult);
            Controls.Add(btnShow);
            Controls.Add(cmbOlympics);
            Controls.Add(lblOlympics);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TopGoldCountryForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = byOlympics ? "Страна с больше всего золотых по олимпиаде" : "Страна с больше всего золотых за всю историю";
            ResumeLayout(false);
            PerformLayout();
        }

        private void LoadOlympics()
        {
            var olympics = dbHelper.GetAllOlympics();
            cmbOlympics.DataSource = olympics.Select(o => new
            {
                o.OlympicsId,
                Display = $"{o.Year} - {o.City} ({o.HostCountryName})"
            }).ToList();
            cmbOlympics.DisplayMember = "Display";
            cmbOlympics.ValueMember = "OlympicsId";
        }

        private void LoadData()
        {
            int? olympicsId = byOlympics && cmbOlympics.SelectedValue != null 
                ? (int)cmbOlympics.SelectedValue 
                : null;

            var country = dbHelper.GetCountryWithMostGoldMedals(olympicsId);
            if (country != null)
            {
                lblResult.Text = $"Страна: {country.CountryName}\n\n" +
                               $"Золото: {country.Gold}\n" +
                               $"Серебро: {country.Silver}\n" +
                               $"Бронза: {country.Bronze}\n" +
                               $"Всего: {country.Total}";
            }
            else
            {
                lblResult.Text = "Данные не найдены";
            }
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (cmbOlympics.SelectedValue == null)
            {
                MessageBox.Show("Выберите олимпиаду", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LoadData();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

