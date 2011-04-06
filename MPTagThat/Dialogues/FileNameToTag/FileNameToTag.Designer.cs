namespace MPTagThat.FileNameToTag
{
  partial class FileNameToTag
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
      this.groupBoxParm = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.lblModifiedBy = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblBPM = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblSubTitle = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblContentGroup = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblComposer = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblConductor = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmFolder = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmUnused = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmComment = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblAlbumArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmGenre = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmDiscTotal = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmDisc = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmTrackTotal = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmTrack = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmYear = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmAlbum = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmTitle = new MPTagThat.Core.WinControls.MPTLabel();
      this.lblParmArtist = new MPTagThat.Core.WinControls.MPTLabel();
      this.btApply = new MPTagThat.Core.WinControls.MPTButton();
      this.btCancel = new MPTagThat.Core.WinControls.MPTButton();
      this.labelHeader = new MPTagThat.Core.WinControls.MPTLabel();
      this.GroupBoxFormat = new MPTagThat.Core.WinControls.MPTGroupBox();
      this.btReview = new MPTagThat.Core.WinControls.MPTButton();
      this.btRemoveFormat = new MPTagThat.Core.WinControls.MPTButton();
      this.btAddFormat = new MPTagThat.Core.WinControls.MPTButton();
      this.cbFormat = new MPTagThat.Core.WinControls.MPTComboBox();
      this.panel1 = new MPTagThat.Core.WinControls.MPTPanel();
      this.tabControl1 = new Elegant.Ui.TabControl();
      this.tabPagePreview = new MPTagThat.Core.WinControls.MPTTabPage();
      this.tabPageParameter = new MPTagThat.Core.WinControls.MPTTabPage();
      this.groupBoxParm.SuspendLayout();
      this.GroupBoxFormat.SuspendLayout();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
      this.tabPageParameter.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxParm
      // 
      this.groupBoxParm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxParm.Controls.Add(this.lblModifiedBy);
      this.groupBoxParm.Controls.Add(this.lblBPM);
      this.groupBoxParm.Controls.Add(this.lblSubTitle);
      this.groupBoxParm.Controls.Add(this.lblContentGroup);
      this.groupBoxParm.Controls.Add(this.lblComposer);
      this.groupBoxParm.Controls.Add(this.lblConductor);
      this.groupBoxParm.Controls.Add(this.lblParmFolder);
      this.groupBoxParm.Controls.Add(this.lblParmUnused);
      this.groupBoxParm.Controls.Add(this.lblParmComment);
      this.groupBoxParm.Controls.Add(this.lblAlbumArtist);
      this.groupBoxParm.Controls.Add(this.lblParmGenre);
      this.groupBoxParm.Controls.Add(this.lblParmDiscTotal);
      this.groupBoxParm.Controls.Add(this.lblParmDisc);
      this.groupBoxParm.Controls.Add(this.lblParmTrackTotal);
      this.groupBoxParm.Controls.Add(this.lblParmTrack);
      this.groupBoxParm.Controls.Add(this.lblParmYear);
      this.groupBoxParm.Controls.Add(this.lblParmAlbum);
      this.groupBoxParm.Controls.Add(this.lblParmTitle);
      this.groupBoxParm.Controls.Add(this.lblParmArtist);
      this.groupBoxParm.Id = "1b0f4c17-fc0a-4968-97bd-ee433c6fe778";
      this.groupBoxParm.Localisation = "GroupBoxParm";
      this.groupBoxParm.LocalisationContext = "TagAndRename";
      this.groupBoxParm.Location = new System.Drawing.Point(8, 9);
      this.groupBoxParm.Name = "groupBoxParm";
      this.groupBoxParm.Size = new System.Drawing.Size(724, 170);
      this.groupBoxParm.TabIndex = 21;
      this.groupBoxParm.Text = "Parameters (Click to add to the list)";
      // 
      // lblModifiedBy
      // 
      this.lblModifiedBy.Localisation = "ModifiedBY";
      this.lblModifiedBy.LocalisationContext = "TagAndRename";
      this.lblModifiedBy.Location = new System.Drawing.Point(6, 119);
      this.lblModifiedBy.Name = "lblModifiedBy";
      this.lblModifiedBy.Size = new System.Drawing.Size(141, 13);
      this.lblModifiedBy.TabIndex = 18;
      this.lblModifiedBy.Text = "<M> = Modified / remixed by";
      this.lblModifiedBy.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblBPM
      // 
      this.lblBPM.Localisation = "BPM";
      this.lblBPM.LocalisationContext = "TagAndRename";
      this.lblBPM.Location = new System.Drawing.Point(271, 119);
      this.lblBPM.Name = "lblBPM";
      this.lblBPM.Size = new System.Drawing.Size(61, 13);
      this.lblBPM.TabIndex = 17;
      this.lblBPM.Text = "<E> = BPM";
      this.lblBPM.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblSubTitle
      // 
      this.lblSubTitle.Localisation = "SubTitle";
      this.lblSubTitle.LocalisationContext = "TagAndRename";
      this.lblSubTitle.Location = new System.Drawing.Point(533, 99);
      this.lblSubTitle.Name = "lblSubTitle";
      this.lblSubTitle.Size = new System.Drawing.Size(77, 13);
      this.lblSubTitle.TabIndex = 16;
      this.lblSubTitle.Text = "<S> = SubTitle";
      this.lblSubTitle.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblContentGroup
      // 
      this.lblContentGroup.Localisation = "Group";
      this.lblContentGroup.LocalisationContext = "TagAndRename";
      this.lblContentGroup.Location = new System.Drawing.Point(533, 78);
      this.lblContentGroup.Name = "lblContentGroup";
      this.lblContentGroup.Size = new System.Drawing.Size(108, 13);
      this.lblContentGroup.TabIndex = 15;
      this.lblContentGroup.Text = "<U> = Content Group";
      this.lblContentGroup.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblComposer
      // 
      this.lblComposer.Localisation = "Composer";
      this.lblComposer.LocalisationContext = "TagAndRename";
      this.lblComposer.Location = new System.Drawing.Point(533, 58);
      this.lblComposer.Name = "lblComposer";
      this.lblComposer.Size = new System.Drawing.Size(86, 13);
      this.lblComposer.TabIndex = 14;
      this.lblComposer.Text = "<R> = Composer";
      this.lblComposer.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblConductor
      // 
      this.lblConductor.Localisation = "Conductor";
      this.lblConductor.LocalisationContext = "TagAndRename";
      this.lblConductor.Location = new System.Drawing.Point(271, 58);
      this.lblConductor.Name = "lblConductor";
      this.lblConductor.Size = new System.Drawing.Size(88, 13);
      this.lblConductor.TabIndex = 13;
      this.lblConductor.Text = "<N> = Conductor";
      this.lblConductor.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmFolder
      // 
      this.lblParmFolder.Localisation = "Folder";
      this.lblParmFolder.LocalisationContext = "TagAndRename";
      this.lblParmFolder.Location = new System.Drawing.Point(11, 143);
      this.lblParmFolder.Name = "lblParmFolder";
      this.lblParmFolder.Size = new System.Drawing.Size(391, 13);
      this.lblParmFolder.TabIndex = 12;
      this.lblParmFolder.Text = "\\ = Folder: to specify that parameters in front of it to be taken from the folder" +
    " name";
      this.lblParmFolder.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmUnused
      // 
      this.lblParmUnused.Localisation = "Unused";
      this.lblParmUnused.LocalisationContext = "TagAndRename";
      this.lblParmUnused.Location = new System.Drawing.Point(533, 119);
      this.lblParmUnused.Name = "lblParmUnused";
      this.lblParmUnused.Size = new System.Drawing.Size(75, 13);
      this.lblParmUnused.TabIndex = 11;
      this.lblParmUnused.Text = "<X> = Unused";
      this.lblParmUnused.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmComment
      // 
      this.lblParmComment.Localisation = "Comment";
      this.lblParmComment.LocalisationContext = "TagAndRename";
      this.lblParmComment.Location = new System.Drawing.Point(533, 38);
      this.lblParmComment.Name = "lblParmComment";
      this.lblParmComment.Size = new System.Drawing.Size(82, 13);
      this.lblParmComment.TabIndex = 10;
      this.lblParmComment.Text = "<C> = Comment";
      this.lblParmComment.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblAlbumArtist
      // 
      this.lblAlbumArtist.Localisation = "AlbumArtist";
      this.lblAlbumArtist.LocalisationContext = "TagAndRename";
      this.lblAlbumArtist.Location = new System.Drawing.Point(6, 58);
      this.lblAlbumArtist.Name = "lblAlbumArtist";
      this.lblAlbumArtist.Size = new System.Drawing.Size(151, 13);
      this.lblAlbumArtist.TabIndex = 9;
      this.lblAlbumArtist.Text = "<O> = Orchestra / Album Artist";
      this.lblAlbumArtist.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmGenre
      // 
      this.lblParmGenre.Localisation = "Genre";
      this.lblParmGenre.LocalisationContext = "TagAndRename";
      this.lblParmGenre.Location = new System.Drawing.Point(271, 38);
      this.lblParmGenre.Name = "lblParmGenre";
      this.lblParmGenre.Size = new System.Drawing.Size(68, 13);
      this.lblParmGenre.TabIndex = 8;
      this.lblParmGenre.Text = "<G> = Genre";
      this.lblParmGenre.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmDiscTotal
      // 
      this.lblParmDiscTotal.Localisation = "DiscTotal";
      this.lblParmDiscTotal.LocalisationContext = "TagAndRename";
      this.lblParmDiscTotal.Location = new System.Drawing.Point(271, 99);
      this.lblParmDiscTotal.Name = "lblParmDiscTotal";
      this.lblParmDiscTotal.Size = new System.Drawing.Size(110, 13);
      this.lblParmDiscTotal.TabIndex = 7;
      this.lblParmDiscTotal.Text = "<d> = Total # of discs";
      this.lblParmDiscTotal.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmDisc
      // 
      this.lblParmDisc.Localisation = "Disc";
      this.lblParmDisc.LocalisationContext = "TagAndRename";
      this.lblParmDisc.Location = new System.Drawing.Point(6, 99);
      this.lblParmDisc.Name = "lblParmDisc";
      this.lblParmDisc.Size = new System.Drawing.Size(100, 13);
      this.lblParmDisc.TabIndex = 6;
      this.lblParmDisc.Text = "<D> = Disc Number";
      this.lblParmDisc.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmTrackTotal
      // 
      this.lblParmTrackTotal.Localisation = "TrackTotal";
      this.lblParmTrackTotal.LocalisationContext = "TagAndRename";
      this.lblParmTrackTotal.Location = new System.Drawing.Point(271, 78);
      this.lblParmTrackTotal.Name = "lblParmTrackTotal";
      this.lblParmTrackTotal.Size = new System.Drawing.Size(115, 13);
      this.lblParmTrackTotal.TabIndex = 5;
      this.lblParmTrackTotal.Text = "<k> = Total # of tracks";
      this.lblParmTrackTotal.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmTrack
      // 
      this.lblParmTrack.Localisation = "Track";
      this.lblParmTrack.LocalisationContext = "TagAndRename";
      this.lblParmTrack.Location = new System.Drawing.Point(6, 78);
      this.lblParmTrack.Name = "lblParmTrack";
      this.lblParmTrack.Size = new System.Drawing.Size(106, 13);
      this.lblParmTrack.TabIndex = 4;
      this.lblParmTrack.Text = "<K> = Track Number";
      this.lblParmTrack.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmYear
      // 
      this.lblParmYear.Localisation = "Year";
      this.lblParmYear.LocalisationContext = "TagAndRename";
      this.lblParmYear.Location = new System.Drawing.Point(6, 38);
      this.lblParmYear.Name = "lblParmYear";
      this.lblParmYear.Size = new System.Drawing.Size(60, 13);
      this.lblParmYear.TabIndex = 3;
      this.lblParmYear.Text = "<Y> = Year";
      this.lblParmYear.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmAlbum
      // 
      this.lblParmAlbum.Localisation = "Album";
      this.lblParmAlbum.LocalisationContext = "TagAndRename";
      this.lblParmAlbum.Location = new System.Drawing.Point(533, 17);
      this.lblParmAlbum.Name = "lblParmAlbum";
      this.lblParmAlbum.Size = new System.Drawing.Size(67, 13);
      this.lblParmAlbum.TabIndex = 2;
      this.lblParmAlbum.Text = "<B> = Album";
      this.lblParmAlbum.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmTitle
      // 
      this.lblParmTitle.Localisation = "Title";
      this.lblParmTitle.LocalisationContext = "TagAndRename";
      this.lblParmTitle.Location = new System.Drawing.Point(271, 16);
      this.lblParmTitle.Name = "lblParmTitle";
      this.lblParmTitle.Size = new System.Drawing.Size(58, 13);
      this.lblParmTitle.TabIndex = 1;
      this.lblParmTitle.Text = "<T> = Title";
      this.lblParmTitle.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // lblParmArtist
      // 
      this.lblParmArtist.Localisation = "Artist";
      this.lblParmArtist.LocalisationContext = "TagAndRename";
      this.lblParmArtist.Location = new System.Drawing.Point(6, 17);
      this.lblParmArtist.Name = "lblParmArtist";
      this.lblParmArtist.Size = new System.Drawing.Size(61, 13);
      this.lblParmArtist.TabIndex = 0;
      this.lblParmArtist.Text = "<A> = Artist";
      this.lblParmArtist.Click += new System.EventHandler(this.lblParm_Click);
      // 
      // btApply
      // 
      this.btApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btApply.Id = "7a3fba3f-8606-41df-ab64-228ba213763e";
      this.btApply.Localisation = "Apply";
      this.btApply.LocalisationContext = "TagAndRename";
      this.btApply.Location = new System.Drawing.Point(552, 22);
      this.btApply.Name = "btApply";
      this.btApply.Size = new System.Drawing.Size(99, 23);
      this.btApply.TabIndex = 1;
      this.btApply.Text = "Apply";
      this.btApply.UseVisualStyleBackColor = true;
      this.btApply.Click += new System.EventHandler(this.btApply_Click);
      // 
      // btCancel
      // 
      this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btCancel.Id = "8a8f88ae-9e30-43c9-afb0-bee2b454009c";
      this.btCancel.Localisation = "Cancel";
      this.btCancel.LocalisationContext = "TagAndRename";
      this.btCancel.Location = new System.Drawing.Point(666, 22);
      this.btCancel.Name = "btCancel";
      this.btCancel.Size = new System.Drawing.Size(99, 23);
      this.btCancel.TabIndex = 2;
      this.btCancel.Text = "Cancel";
      this.btCancel.UseVisualStyleBackColor = true;
      this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
      // 
      // labelHeader
      // 
      this.labelHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelHeader.ForeColor = System.Drawing.Color.White;
      this.labelHeader.Localisation = "labelHeader";
      this.labelHeader.LocalisationContext = "FileNameToTag";
      this.labelHeader.Location = new System.Drawing.Point(24, 22);
      this.labelHeader.Name = "labelHeader";
      this.labelHeader.Size = new System.Drawing.Size(62, 20);
      this.labelHeader.TabIndex = 22;
      this.labelHeader.Text = "Header";
      // 
      // GroupBoxFormat
      // 
      this.GroupBoxFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GroupBoxFormat.Controls.Add(this.btReview);
      this.GroupBoxFormat.Controls.Add(this.btRemoveFormat);
      this.GroupBoxFormat.Controls.Add(this.btAddFormat);
      this.GroupBoxFormat.Controls.Add(this.cbFormat);
      this.GroupBoxFormat.Id = "324e69c5-3e43-4bc9-a6a6-b0f2018f0cea";
      this.GroupBoxFormat.Localisation = "GroupBoxFormat";
      this.GroupBoxFormat.LocalisationContext = "FileNameToTag";
      this.GroupBoxFormat.Location = new System.Drawing.Point(19, 54);
      this.GroupBoxFormat.Name = "GroupBoxFormat";
      this.GroupBoxFormat.Size = new System.Drawing.Size(744, 83);
      this.GroupBoxFormat.TabIndex = 23;
      this.GroupBoxFormat.Text = "Format";
      // 
      // btReview
      // 
      this.btReview.Id = "18c053af-42f5-4ecb-8ca7-3af4975be81d";
      this.btReview.Localisation = "Preview";
      this.btReview.LocalisationContext = "TagAndRename";
      this.btReview.Location = new System.Drawing.Point(421, 49);
      this.btReview.Name = "btReview";
      this.btReview.Size = new System.Drawing.Size(200, 23);
      this.btReview.TabIndex = 17;
      this.btReview.Text = "Preview Changes";
      this.btReview.UseVisualStyleBackColor = true;
      this.btReview.Click += new System.EventHandler(this.btReview_Click);
      // 
      // btRemoveFormat
      // 
      this.btRemoveFormat.Id = "cf59aab0-f07f-457c-9dbd-1f5bb12eca27";
      this.btRemoveFormat.Localisation = "RemoveFormat";
      this.btRemoveFormat.LocalisationContext = "TagAndRename";
      this.btRemoveFormat.Location = new System.Drawing.Point(215, 49);
      this.btRemoveFormat.Name = "btRemoveFormat";
      this.btRemoveFormat.Size = new System.Drawing.Size(200, 23);
      this.btRemoveFormat.TabIndex = 16;
      this.btRemoveFormat.Text = "Remove Format From List";
      this.btRemoveFormat.UseVisualStyleBackColor = true;
      this.btRemoveFormat.Click += new System.EventHandler(this.btRemoveFormat_Click);
      // 
      // btAddFormat
      // 
      this.btAddFormat.Id = "3c061c33-44f6-42f1-abdc-6bdf9c7eaf2a";
      this.btAddFormat.Localisation = "AddFormat";
      this.btAddFormat.LocalisationContext = "TagAndRename";
      this.btAddFormat.Location = new System.Drawing.Point(9, 49);
      this.btAddFormat.Name = "btAddFormat";
      this.btAddFormat.Size = new System.Drawing.Size(200, 23);
      this.btAddFormat.TabIndex = 15;
      this.btAddFormat.Text = "Add Format To List";
      this.btAddFormat.UseVisualStyleBackColor = true;
      this.btAddFormat.Click += new System.EventHandler(this.btAddFormat_Click);
      // 
      // cbFormat
      // 
      this.cbFormat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.cbFormat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.cbFormat.FormattingEnabled = true;
      this.cbFormat.Id = "3f0c920e-4209-4cd6-ab23-12dc30a5e3f6";
      this.cbFormat.Location = new System.Drawing.Point(9, 22);
      this.cbFormat.Name = "cbFormat";
      this.cbFormat.Size = new System.Drawing.Size(610, 21);
      this.cbFormat.TabIndex = 14;
      this.cbFormat.TextEditorWidth = 591;
      this.cbFormat.TextChanged += new System.EventHandler(this.cbFormat_TextChanged);
      this.cbFormat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbFormat_Keypress);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.tabControl1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(780, 375);
      this.panel1.TabIndex = 24;
      // 
      // tabControl1
      // 
      this.tabControl1.Location = new System.Drawing.Point(19, 154);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedTabPage = this.tabPagePreview;
      this.tabControl1.Size = new System.Drawing.Size(744, 210);
      this.tabControl1.TabIndex = 22;
      this.tabControl1.TabPages.AddRange(new Elegant.Ui.TabPage[] {
            this.tabPageParameter,
            this.tabPagePreview});
      this.tabControl1.Tag = "Preview";
      this.tabControl1.Text = "tabControl1";
      this.tabControl1.SelectedTabPageChanged += new Elegant.Ui.TabPageChangedEventHandler(this.tabControl1_SelectedTabPageChanged);
      // 
      // tabPagePreview
      // 
      this.tabPagePreview.ActiveControl = null;
      this.tabPagePreview.KeyTip = null;
      this.tabPagePreview.Localisation = "Preview";
      this.tabPagePreview.LocalisationContext = "TagAndRename";
      this.tabPagePreview.Name = "tabPagePreview";
      this.tabPagePreview.Size = new System.Drawing.Size(742, 189);
      this.tabPagePreview.TabIndex = 1;
      this.tabPagePreview.Text = "Preview";
      // 
      // tabPageParameter
      // 
      this.tabPageParameter.ActiveControl = null;
      this.tabPageParameter.Controls.Add(this.groupBoxParm);
      this.tabPageParameter.KeyTip = null;
      this.tabPageParameter.Localisation = "Parameters";
      this.tabPageParameter.LocalisationContext = "TagAndRename";
      this.tabPageParameter.Name = "tabPageParameter";
      this.tabPageParameter.Size = new System.Drawing.Size(742, 189);
      this.tabPageParameter.TabIndex = 0;
      this.tabPageParameter.Tag = "Parameter";
      this.tabPageParameter.Text = "Parameters";
      // 
      // FileNameToTag
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.Controls.Add(this.GroupBoxFormat);
      this.Controls.Add(this.labelHeader);
      this.Controls.Add(this.btCancel);
      this.Controls.Add(this.btApply);
      this.Controls.Add(this.panel1);
      this.MaximumSize = new System.Drawing.Size(780, 375);
      this.MinimumSize = new System.Drawing.Size(780, 375);
      this.Name = "FileNameToTag";
      this.Size = new System.Drawing.Size(780, 375);
      this.groupBoxParm.ResumeLayout(false);
      this.groupBoxParm.PerformLayout();
      this.GroupBoxFormat.ResumeLayout(false);
      this.GroupBoxFormat.PerformLayout();
      this.panel1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
      this.tabPageParameter.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MPTagThat.Core.WinControls.MPTGroupBox groupBoxParm;
    private MPTagThat.Core.WinControls.MPTLabel lblParmFolder;
    private MPTagThat.Core.WinControls.MPTLabel lblParmUnused;
    private MPTagThat.Core.WinControls.MPTLabel lblParmComment;
    private MPTagThat.Core.WinControls.MPTLabel lblAlbumArtist;
    private MPTagThat.Core.WinControls.MPTLabel lblParmGenre;
    private MPTagThat.Core.WinControls.MPTLabel lblParmDiscTotal;
    private MPTagThat.Core.WinControls.MPTLabel lblParmDisc;
    private MPTagThat.Core.WinControls.MPTLabel lblParmTrackTotal;
    private MPTagThat.Core.WinControls.MPTLabel lblParmTrack;
    private MPTagThat.Core.WinControls.MPTLabel lblParmYear;
    private MPTagThat.Core.WinControls.MPTLabel lblParmAlbum;
    private MPTagThat.Core.WinControls.MPTLabel lblParmTitle;
    private MPTagThat.Core.WinControls.MPTLabel lblParmArtist;
    private MPTagThat.Core.WinControls.MPTButton btApply;
    private MPTagThat.Core.WinControls.MPTButton btCancel;
    private MPTagThat.Core.WinControls.MPTLabel lblConductor;
    private MPTagThat.Core.WinControls.MPTLabel lblModifiedBy;
    private MPTagThat.Core.WinControls.MPTLabel lblBPM;
    private MPTagThat.Core.WinControls.MPTLabel lblSubTitle;
    private MPTagThat.Core.WinControls.MPTLabel lblContentGroup;
    private MPTagThat.Core.WinControls.MPTLabel lblComposer;
    private MPTagThat.Core.WinControls.MPTLabel labelHeader;
    private MPTagThat.Core.WinControls.MPTGroupBox GroupBoxFormat;
    private MPTagThat.Core.WinControls.MPTButton btRemoveFormat;
    private MPTagThat.Core.WinControls.MPTButton btAddFormat;
    private MPTagThat.Core.WinControls.MPTComboBox cbFormat;
    private MPTagThat.Core.WinControls.MPTButton btReview;
    private Core.WinControls.MPTPanel panel1;
    private Elegant.Ui.TabControl tabControl1;
    private MPTagThat.Core.WinControls.MPTTabPage tabPageParameter;
    private MPTagThat.Core.WinControls.MPTTabPage tabPagePreview;
  }
}