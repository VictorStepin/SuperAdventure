namespace SuperAdventure
{
    partial class QuestLogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvQuests = new System.Windows.Forms.DataGridView();
            this.QuestName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuestDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsCompleted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnHide = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuests)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvQuests
            // 
            this.dgvQuests.AllowUserToAddRows = false;
            this.dgvQuests.AllowUserToDeleteRows = false;
            this.dgvQuests.AllowUserToResizeColumns = false;
            this.dgvQuests.AllowUserToResizeRows = false;
            this.dgvQuests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQuests.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.QuestName,
            this.QuestDescription,
            this.IsCompleted});
            this.dgvQuests.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvQuests.Location = new System.Drawing.Point(12, 12);
            this.dgvQuests.MultiSelect = false;
            this.dgvQuests.Name = "dgvQuests";
            this.dgvQuests.ReadOnly = true;
            this.dgvQuests.RowHeadersVisible = false;
            this.dgvQuests.RowHeadersWidth = 51;
            this.dgvQuests.RowTemplate.Height = 29;
            this.dgvQuests.Size = new System.Drawing.Size(658, 483);
            this.dgvQuests.TabIndex = 0;
            // 
            // QuestName
            // 
            this.QuestName.HeaderText = "Name";
            this.QuestName.MinimumWidth = 6;
            this.QuestName.Name = "QuestName";
            this.QuestName.ReadOnly = true;
            this.QuestName.Width = 200;
            // 
            // QuestDescription
            // 
            this.QuestDescription.HeaderText = "Description";
            this.QuestDescription.MinimumWidth = 6;
            this.QuestDescription.Name = "QuestDescription";
            this.QuestDescription.ReadOnly = true;
            this.QuestDescription.Width = 325;
            // 
            // IsCompleted
            // 
            this.IsCompleted.HeaderText = "IsCompleted";
            this.IsCompleted.MinimumWidth = 6;
            this.IsCompleted.Name = "IsCompleted";
            this.IsCompleted.ReadOnly = true;
            this.IsCompleted.Width = 125;
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(676, 12);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(94, 29);
            this.btnHide.TabIndex = 1;
            this.btnHide.Text = "Hide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // QuestLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 507);
            this.Controls.Add(this.btnHide);
            this.Controls.Add(this.dgvQuests);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "QuestLogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quest Log";
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuests)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvQuests;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuestName;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuestDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsCompleted;
    }
}