using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NUnit.Util;

namespace NUnit.UiKit
{
	/// <summary>
	/// ConfigurationEditor form is designed for adding, deleting
	/// and renaming configurations from a project.
	/// </summary>
	public class ConfigurationEditor : System.Windows.Forms.Form
	{
		#region Instance Variables

		private NUnitProject project;

		private int selectedIndex = -1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListBox configListBox;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button renameButton;
		private System.Windows.Forms.Button closeButton;

		#endregion

		#region Construction and Disposal

		public ConfigurationEditor( NUnitProject project )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.project = project;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.configListBox = new System.Windows.Forms.ListBox();
			this.removeButton = new System.Windows.Forms.Button();
			this.renameButton = new System.Windows.Forms.Button();
			this.closeButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// configListBox
			// 
			this.configListBox.ItemHeight = 16;
			this.configListBox.Location = new System.Drawing.Point(8, 8);
			this.configListBox.Name = "configListBox";
			this.configListBox.Size = new System.Drawing.Size(168, 244);
			this.configListBox.TabIndex = 0;
			this.configListBox.SelectedIndexChanged += new System.EventHandler(this.configListBox_SelectedIndexChanged);
			// 
			// removeButton
			// 
			this.removeButton.Location = new System.Drawing.Point(192, 8);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(96, 32);
			this.removeButton.TabIndex = 1;
			this.removeButton.Text = "&Remove";
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// renameButton
			// 
			this.renameButton.Location = new System.Drawing.Point(192, 48);
			this.renameButton.Name = "renameButton";
			this.renameButton.Size = new System.Drawing.Size(96, 32);
			this.renameButton.TabIndex = 2;
			this.renameButton.Text = "Re&name";
			this.renameButton.Click += new System.EventHandler(this.renameButton_Click);
			// 
			// closeButton
			// 
			this.closeButton.Location = new System.Drawing.Point(192, 216);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(96, 32);
			this.closeButton.TabIndex = 4;
			this.closeButton.Text = "Close";
			this.closeButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// ConfigurationEditor
			// 
			this.AcceptButton = this.closeButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(298, 268);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.closeButton,
																		  this.renameButton,
																		  this.removeButton,
																		  this.configListBox});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ConfigurationEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ConfigurationEditor";
			this.Load += new System.EventHandler(this.ConfigurationEditor_Load);
			this.ResumeLayout(false);

		}
		#endregion

		#region Methods

		public static DialogResult Edit( NUnitProject project )
		{
			ConfigurationEditor editor = new ConfigurationEditor( project );
			return editor.ShowDialog();
		}

		private void ConfigurationEditor_Load(object sender, System.EventArgs e)
		{
			FillListBox();
			if ( configListBox.Items.Count > 0 )
				configListBox.SelectedIndex = selectedIndex = 0;
		}

		private void removeButton_Click(object sender, System.EventArgs e)
		{	
			project.Configs.RemoveAt( selectedIndex );
			FillListBox();
		}

		private void renameButton_Click(object sender, System.EventArgs e)
		{
			//Rename( project, configListBox.SelectedIndex );
			int index = configListBox.SelectedIndex;
			string oldname = project.Configs[index].Name;

			RenameConfigurationDialog dlg	= new RenameConfigurationDialog( project, oldname );
			if ( dlg.ShowDialog() == DialogResult.OK )
			{
				project.RenameConfiguration( oldname, dlg.ConfigurationName );
				FillListBox();
			}
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void configListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			selectedIndex = configListBox.SelectedIndex;
			renameButton.Enabled = !project.Configs[selectedIndex].Active;
		}

		private void FillListBox()
		{
			configListBox.Items.Clear();

			foreach( ProjectConfig config in project.Configs )
			{
				string name = config.Name;

				if ( config.Active )
					name += " (active)";
				
				configListBox.Items.Add( name );
			}			
		}

		#endregion
	}
}
