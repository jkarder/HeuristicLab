#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2008 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

namespace HeuristicLab.ES {
  partial class ESEditor {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (chooseOperatorDialog != null) chooseOperatorDialog.Dispose();
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ESEditor));
      this.executeButton = new System.Windows.Forms.Button();
      this.tabControl = new System.Windows.Forms.TabControl();
      this.parametersTabPage = new System.Windows.Forms.TabPage();
      this.leftBracketLabelRight = new System.Windows.Forms.Label();
      this.semicolonLabel = new System.Windows.Forms.Label();
      this.leftBracketLabelLeft = new System.Windows.Forms.Label();
      this.shakingFactorsUpperBoundTextBox = new System.Windows.Forms.TextBox();
      this.successRuleGroupBox = new System.Windows.Forms.GroupBox();
      this.learningRateTextBox = new System.Windows.Forms.TextBox();
      this.learningRateLabel = new System.Windows.Forms.Label();
      this.generalLearningRateTextBox = new System.Windows.Forms.TextBox();
      this.generalLearningRateLabel = new System.Windows.Forms.Label();
      this.learningRateVariableNameLabel = new System.Windows.Forms.Label();
      this.generalLearningRateVariableNameLabel = new System.Windows.Forms.Label();
      this.parentSelectionGroupBox = new System.Windows.Forms.GroupBox();
      this.commaRadioButton = new System.Windows.Forms.RadioButton();
      this.plusRadioButton = new System.Windows.Forms.RadioButton();
      this.setRecombinationButton = new System.Windows.Forms.Button();
      this.viewRecombinationButton = new System.Windows.Forms.Button();
      this.recombinationTextBox = new System.Windows.Forms.TextBox();
      this.recombinationLabel = new System.Windows.Forms.Label();
      this.rhoTextBox = new System.Windows.Forms.TextBox();
      this.rhoLabel = new System.Windows.Forms.Label();
      this.setEvaluationButton = new System.Windows.Forms.Button();
      this.setMutationButton = new System.Windows.Forms.Button();
      this.setSolutionGenerationButton = new System.Windows.Forms.Button();
      this.viewEvaluationButton = new System.Windows.Forms.Button();
      this.viewMutationButton = new System.Windows.Forms.Button();
      this.viewSolutionGenerationButton = new System.Windows.Forms.Button();
      this.viewProblemInitializationButton = new System.Windows.Forms.Button();
      this.setProblemInitializationButton = new System.Windows.Forms.Button();
      this.evaluationTextBox = new System.Windows.Forms.TextBox();
      this.mutationTextBox = new System.Windows.Forms.TextBox();
      this.solutionGenerationTextBox = new System.Windows.Forms.TextBox();
      this.problemInitializationTextBox = new System.Windows.Forms.TextBox();
      this.setRandomSeedRandomlyCheckBox = new System.Windows.Forms.CheckBox();
      this.problemDimensionTextBox = new System.Windows.Forms.TextBox();
      this.shakingFactorsLowerBoundTextBox = new System.Windows.Forms.TextBox();
      this.evaluationLabel = new System.Windows.Forms.Label();
      this.mutationLabel = new System.Windows.Forms.Label();
      this.solutionGenerationLabel = new System.Windows.Forms.Label();
      this.problemInitializationLabel = new System.Windows.Forms.Label();
      this.problemDimensionLabel = new System.Windows.Forms.Label();
      this.mutationStrengthLabel = new System.Windows.Forms.Label();
      this.mutationRateLabel = new System.Windows.Forms.Label();
      this.maximumGenerationsTextBox = new System.Windows.Forms.TextBox();
      this.maximumGenerationsLabel = new System.Windows.Forms.Label();
      this.randomSeedTextBox = new System.Windows.Forms.TextBox();
      this.muTextBox = new System.Windows.Forms.TextBox();
      this.setRandomSeedRandomlyLabel = new System.Windows.Forms.Label();
      this.randomSeedLabel = new System.Windows.Forms.Label();
      this.problemDimensionVariableNameLabel = new System.Windows.Forms.Label();
      this.shakingFactorsVariableNameLabel = new System.Windows.Forms.Label();
      this.maximumGenerationsVariableNameLabel = new System.Windows.Forms.Label();
      this.esLambdaVariableNameLabel = new System.Windows.Forms.Label();
      this.esRhoVariableNameLabel = new System.Windows.Forms.Label();
      this.esMuVariableNameLabel = new System.Windows.Forms.Label();
      this.populationSizeLabel = new System.Windows.Forms.Label();
      this.lambdaTextBox = new System.Windows.Forms.TextBox();
      this.scopesTabPage = new System.Windows.Forms.TabPage();
      this.scopeView = new HeuristicLab.Core.ScopeView();
      this.abortButton = new System.Windows.Forms.Button();
      this.resetButton = new System.Windows.Forms.Button();
      this.cloneEngineButton = new System.Windows.Forms.Button();
      this.toolTip = new System.Windows.Forms.ToolTip(this.components);
      this.tabControl.SuspendLayout();
      this.parametersTabPage.SuspendLayout();
      this.successRuleGroupBox.SuspendLayout();
      this.parentSelectionGroupBox.SuspendLayout();
      this.scopesTabPage.SuspendLayout();
      this.SuspendLayout();
      // 
      // executeButton
      // 
      this.executeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.executeButton.Location = new System.Drawing.Point(0, 527);
      this.executeButton.Name = "executeButton";
      this.executeButton.Size = new System.Drawing.Size(75, 23);
      this.executeButton.TabIndex = 1;
      this.executeButton.Text = "&Execute";
      this.executeButton.UseVisualStyleBackColor = true;
      this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
      // 
      // tabControl
      // 
      this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl.Controls.Add(this.parametersTabPage);
      this.tabControl.Controls.Add(this.scopesTabPage);
      this.tabControl.Location = new System.Drawing.Point(0, 0);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new System.Drawing.Size(501, 521);
      this.tabControl.TabIndex = 0;
      // 
      // parametersTabPage
      // 
      this.parametersTabPage.Controls.Add(this.leftBracketLabelRight);
      this.parametersTabPage.Controls.Add(this.semicolonLabel);
      this.parametersTabPage.Controls.Add(this.leftBracketLabelLeft);
      this.parametersTabPage.Controls.Add(this.shakingFactorsUpperBoundTextBox);
      this.parametersTabPage.Controls.Add(this.successRuleGroupBox);
      this.parametersTabPage.Controls.Add(this.parentSelectionGroupBox);
      this.parametersTabPage.Controls.Add(this.setRecombinationButton);
      this.parametersTabPage.Controls.Add(this.viewRecombinationButton);
      this.parametersTabPage.Controls.Add(this.recombinationTextBox);
      this.parametersTabPage.Controls.Add(this.recombinationLabel);
      this.parametersTabPage.Controls.Add(this.rhoTextBox);
      this.parametersTabPage.Controls.Add(this.rhoLabel);
      this.parametersTabPage.Controls.Add(this.setEvaluationButton);
      this.parametersTabPage.Controls.Add(this.setMutationButton);
      this.parametersTabPage.Controls.Add(this.setSolutionGenerationButton);
      this.parametersTabPage.Controls.Add(this.viewEvaluationButton);
      this.parametersTabPage.Controls.Add(this.viewMutationButton);
      this.parametersTabPage.Controls.Add(this.viewSolutionGenerationButton);
      this.parametersTabPage.Controls.Add(this.viewProblemInitializationButton);
      this.parametersTabPage.Controls.Add(this.setProblemInitializationButton);
      this.parametersTabPage.Controls.Add(this.evaluationTextBox);
      this.parametersTabPage.Controls.Add(this.mutationTextBox);
      this.parametersTabPage.Controls.Add(this.solutionGenerationTextBox);
      this.parametersTabPage.Controls.Add(this.problemInitializationTextBox);
      this.parametersTabPage.Controls.Add(this.setRandomSeedRandomlyCheckBox);
      this.parametersTabPage.Controls.Add(this.problemDimensionTextBox);
      this.parametersTabPage.Controls.Add(this.shakingFactorsLowerBoundTextBox);
      this.parametersTabPage.Controls.Add(this.evaluationLabel);
      this.parametersTabPage.Controls.Add(this.mutationLabel);
      this.parametersTabPage.Controls.Add(this.solutionGenerationLabel);
      this.parametersTabPage.Controls.Add(this.problemInitializationLabel);
      this.parametersTabPage.Controls.Add(this.problemDimensionLabel);
      this.parametersTabPage.Controls.Add(this.mutationStrengthLabel);
      this.parametersTabPage.Controls.Add(this.mutationRateLabel);
      this.parametersTabPage.Controls.Add(this.maximumGenerationsTextBox);
      this.parametersTabPage.Controls.Add(this.maximumGenerationsLabel);
      this.parametersTabPage.Controls.Add(this.randomSeedTextBox);
      this.parametersTabPage.Controls.Add(this.muTextBox);
      this.parametersTabPage.Controls.Add(this.setRandomSeedRandomlyLabel);
      this.parametersTabPage.Controls.Add(this.randomSeedLabel);
      this.parametersTabPage.Controls.Add(this.problemDimensionVariableNameLabel);
      this.parametersTabPage.Controls.Add(this.shakingFactorsVariableNameLabel);
      this.parametersTabPage.Controls.Add(this.maximumGenerationsVariableNameLabel);
      this.parametersTabPage.Controls.Add(this.esLambdaVariableNameLabel);
      this.parametersTabPage.Controls.Add(this.esRhoVariableNameLabel);
      this.parametersTabPage.Controls.Add(this.esMuVariableNameLabel);
      this.parametersTabPage.Controls.Add(this.populationSizeLabel);
      this.parametersTabPage.Controls.Add(this.lambdaTextBox);
      this.parametersTabPage.Location = new System.Drawing.Point(4, 22);
      this.parametersTabPage.Name = "parametersTabPage";
      this.parametersTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.parametersTabPage.Size = new System.Drawing.Size(493, 495);
      this.parametersTabPage.TabIndex = 0;
      this.parametersTabPage.Text = "Parameters";
      this.parametersTabPage.UseVisualStyleBackColor = true;
      // 
      // leftBracketLabelRight
      // 
      this.leftBracketLabelRight.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.leftBracketLabelRight.AutoSize = true;
      this.leftBracketLabelRight.Location = new System.Drawing.Point(344, 175);
      this.leftBracketLabelRight.Name = "leftBracketLabelRight";
      this.leftBracketLabelRight.Size = new System.Drawing.Size(10, 13);
      this.leftBracketLabelRight.TabIndex = 21;
      this.leftBracketLabelRight.Text = "[";
      // 
      // semicolonLabel
      // 
      this.semicolonLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.semicolonLabel.AutoSize = true;
      this.semicolonLabel.Location = new System.Drawing.Point(254, 175);
      this.semicolonLabel.Name = "semicolonLabel";
      this.semicolonLabel.Size = new System.Drawing.Size(10, 13);
      this.semicolonLabel.TabIndex = 19;
      this.semicolonLabel.Text = ";";
      // 
      // leftBracketLabelLeft
      // 
      this.leftBracketLabelLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.leftBracketLabelLeft.AutoSize = true;
      this.leftBracketLabelLeft.Location = new System.Drawing.Point(165, 175);
      this.leftBracketLabelLeft.Name = "leftBracketLabelLeft";
      this.leftBracketLabelLeft.Size = new System.Drawing.Size(10, 13);
      this.leftBracketLabelLeft.TabIndex = 17;
      this.leftBracketLabelLeft.Text = "[";
      // 
      // shakingFactorsUpperBoundTextBox
      // 
      this.shakingFactorsUpperBoundTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.shakingFactorsUpperBoundTextBox.Location = new System.Drawing.Point(273, 172);
      this.shakingFactorsUpperBoundTextBox.Name = "shakingFactorsUpperBoundTextBox";
      this.shakingFactorsUpperBoundTextBox.Size = new System.Drawing.Size(65, 20);
      this.shakingFactorsUpperBoundTextBox.TabIndex = 20;
      this.toolTip.SetToolTip(this.shakingFactorsUpperBoundTextBox, resources.GetString("shakingFactorsUpperBoundTextBox.ToolTip"));
      // 
      // successRuleGroupBox
      // 
      this.successRuleGroupBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.successRuleGroupBox.Controls.Add(this.learningRateTextBox);
      this.successRuleGroupBox.Controls.Add(this.learningRateLabel);
      this.successRuleGroupBox.Controls.Add(this.generalLearningRateTextBox);
      this.successRuleGroupBox.Controls.Add(this.generalLearningRateLabel);
      this.successRuleGroupBox.Controls.Add(this.learningRateVariableNameLabel);
      this.successRuleGroupBox.Controls.Add(this.generalLearningRateVariableNameLabel);
      this.successRuleGroupBox.Location = new System.Drawing.Point(9, 230);
      this.successRuleGroupBox.Name = "successRuleGroupBox";
      this.successRuleGroupBox.Size = new System.Drawing.Size(476, 74);
      this.successRuleGroupBox.TabIndex = 26;
      this.successRuleGroupBox.TabStop = false;
      this.successRuleGroupBox.Text = "Mutation Strength Adjustment";
      // 
      // learningRateTextBox
      // 
      this.learningRateTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.learningRateTextBox.Location = new System.Drawing.Point(159, 45);
      this.learningRateTextBox.Name = "learningRateTextBox";
      this.learningRateTextBox.Size = new System.Drawing.Size(186, 20);
      this.learningRateTextBox.TabIndex = 4;
      this.toolTip.SetToolTip(this.learningRateTextBox, "Automatically updates when Problem Dimension (dim) is changed.\r\nThe used formula " +
              "is: tau = 1 / Sqrt(2*Sqrt(dim))");
      // 
      // learningRateLabel
      // 
      this.learningRateLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.learningRateLabel.AutoSize = true;
      this.learningRateLabel.Location = new System.Drawing.Point(7, 48);
      this.learningRateLabel.Name = "learningRateLabel";
      this.learningRateLabel.Size = new System.Drawing.Size(92, 13);
      this.learningRateLabel.TabIndex = 3;
      this.learningRateLabel.Text = "Learning Rate (τ):";
      // 
      // generalLearningRateTextBox
      // 
      this.generalLearningRateTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.generalLearningRateTextBox.Location = new System.Drawing.Point(159, 19);
      this.generalLearningRateTextBox.Name = "generalLearningRateTextBox";
      this.generalLearningRateTextBox.Size = new System.Drawing.Size(186, 20);
      this.generalLearningRateTextBox.TabIndex = 1;
      this.toolTip.SetToolTip(this.generalLearningRateTextBox, "Automatically updates when Problem Dimension (dim) is changed.\r\nThe used formula " +
              "is: tau0 = 1 / Sqrt(2*dim)");
      // 
      // generalLearningRateLabel
      // 
      this.generalLearningRateLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.generalLearningRateLabel.AutoSize = true;
      this.generalLearningRateLabel.Location = new System.Drawing.Point(7, 22);
      this.generalLearningRateLabel.Name = "generalLearningRateLabel";
      this.generalLearningRateLabel.Size = new System.Drawing.Size(138, 13);
      this.generalLearningRateLabel.TabIndex = 0;
      this.generalLearningRateLabel.Text = "General Learning Rate (τ0):";
      // 
      // learningRateVariableNameLabel
      // 
      this.learningRateVariableNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.learningRateVariableNameLabel.AutoSize = true;
      this.learningRateVariableNameLabel.Location = new System.Drawing.Point(348, 48);
      this.learningRateVariableNameLabel.Name = "learningRateVariableNameLabel";
      this.learningRateVariableNameLabel.Size = new System.Drawing.Size(77, 13);
      this.learningRateVariableNameLabel.TabIndex = 5;
      this.learningRateVariableNameLabel.Text = "(LearningRate)";
      // 
      // generalLearningRateVariableNameLabel
      // 
      this.generalLearningRateVariableNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.generalLearningRateVariableNameLabel.AutoSize = true;
      this.generalLearningRateVariableNameLabel.Location = new System.Drawing.Point(348, 22);
      this.generalLearningRateVariableNameLabel.Name = "generalLearningRateVariableNameLabel";
      this.generalLearningRateVariableNameLabel.Size = new System.Drawing.Size(114, 13);
      this.generalLearningRateVariableNameLabel.TabIndex = 2;
      this.generalLearningRateVariableNameLabel.Text = "(GeneralLearningRate)";
      // 
      // parentSelectionGroupBox
      // 
      this.parentSelectionGroupBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.parentSelectionGroupBox.Controls.Add(this.commaRadioButton);
      this.parentSelectionGroupBox.Controls.Add(this.plusRadioButton);
      this.parentSelectionGroupBox.Location = new System.Drawing.Point(168, 310);
      this.parentSelectionGroupBox.Name = "parentSelectionGroupBox";
      this.parentSelectionGroupBox.Size = new System.Drawing.Size(186, 38);
      this.parentSelectionGroupBox.TabIndex = 27;
      this.parentSelectionGroupBox.TabStop = false;
      this.parentSelectionGroupBox.Text = "Parent Selection";
      // 
      // commaRadioButton
      // 
      this.commaRadioButton.AutoSize = true;
      this.commaRadioButton.Location = new System.Drawing.Point(57, 15);
      this.commaRadioButton.Name = "commaRadioButton";
      this.commaRadioButton.Size = new System.Drawing.Size(60, 17);
      this.commaRadioButton.TabIndex = 1;
      this.commaRadioButton.Text = "Comma";
      this.commaRadioButton.UseVisualStyleBackColor = true;
      this.commaRadioButton.CheckedChanged += new System.EventHandler(this.commaRadioButton_CheckedChanged);
      // 
      // plusRadioButton
      // 
      this.plusRadioButton.AutoSize = true;
      this.plusRadioButton.Checked = true;
      this.plusRadioButton.Location = new System.Drawing.Point(6, 15);
      this.plusRadioButton.Name = "plusRadioButton";
      this.plusRadioButton.Size = new System.Drawing.Size(45, 17);
      this.plusRadioButton.TabIndex = 0;
      this.plusRadioButton.TabStop = true;
      this.plusRadioButton.Text = "Plus";
      this.plusRadioButton.UseVisualStyleBackColor = true;
      this.plusRadioButton.CheckedChanged += new System.EventHandler(this.plusRadioButton_CheckedChanged);
      // 
      // setRecombinationButton
      // 
      this.setRecombinationButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.setRecombinationButton.Location = new System.Drawing.Point(419, 458);
      this.setRecombinationButton.Name = "setRecombinationButton";
      this.setRecombinationButton.Size = new System.Drawing.Size(43, 20);
      this.setRecombinationButton.TabIndex = 47;
      this.setRecombinationButton.Text = "Set...";
      this.setRecombinationButton.UseVisualStyleBackColor = true;
      this.setRecombinationButton.Click += new System.EventHandler(this.setRecombinationButton_Click);
      // 
      // viewRecombinationButton
      // 
      this.viewRecombinationButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.viewRecombinationButton.Location = new System.Drawing.Point(360, 458);
      this.viewRecombinationButton.Name = "viewRecombinationButton";
      this.viewRecombinationButton.Size = new System.Drawing.Size(53, 20);
      this.viewRecombinationButton.TabIndex = 46;
      this.viewRecombinationButton.Text = "View...";
      this.viewRecombinationButton.UseVisualStyleBackColor = true;
      this.viewRecombinationButton.Click += new System.EventHandler(this.viewRecombinationButton_Click);
      // 
      // recombinationTextBox
      // 
      this.recombinationTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.recombinationTextBox.Location = new System.Drawing.Point(168, 458);
      this.recombinationTextBox.Name = "recombinationTextBox";
      this.recombinationTextBox.ReadOnly = true;
      this.recombinationTextBox.Size = new System.Drawing.Size(186, 20);
      this.recombinationTextBox.TabIndex = 45;
      // 
      // recombinationLabel
      // 
      this.recombinationLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.recombinationLabel.AutoSize = true;
      this.recombinationLabel.Location = new System.Drawing.Point(6, 461);
      this.recombinationLabel.Name = "recombinationLabel";
      this.recombinationLabel.Size = new System.Drawing.Size(81, 13);
      this.recombinationLabel.TabIndex = 44;
      this.recombinationLabel.Text = "Recombination:";
      // 
      // rhoTextBox
      // 
      this.rhoTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.rhoTextBox.Location = new System.Drawing.Point(168, 94);
      this.rhoTextBox.Name = "rhoTextBox";
      this.rhoTextBox.Size = new System.Drawing.Size(186, 20);
      this.rhoTextBox.TabIndex = 8;
      // 
      // rhoLabel
      // 
      this.rhoLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.rhoLabel.AutoSize = true;
      this.rhoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rhoLabel.Location = new System.Drawing.Point(6, 97);
      this.rhoLabel.Name = "rhoLabel";
      this.rhoLabel.Size = new System.Drawing.Size(45, 13);
      this.rhoLabel.TabIndex = 7;
      this.rhoLabel.Text = "Rho (ρ):";
      // 
      // setEvaluationButton
      // 
      this.setEvaluationButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.setEvaluationButton.Location = new System.Drawing.Point(419, 432);
      this.setEvaluationButton.Name = "setEvaluationButton";
      this.setEvaluationButton.Size = new System.Drawing.Size(43, 20);
      this.setEvaluationButton.TabIndex = 43;
      this.setEvaluationButton.Text = "Set...";
      this.setEvaluationButton.UseVisualStyleBackColor = true;
      this.setEvaluationButton.Click += new System.EventHandler(this.setEvaluationButton_Click);
      // 
      // setMutationButton
      // 
      this.setMutationButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.setMutationButton.Location = new System.Drawing.Point(419, 406);
      this.setMutationButton.Name = "setMutationButton";
      this.setMutationButton.Size = new System.Drawing.Size(43, 20);
      this.setMutationButton.TabIndex = 39;
      this.setMutationButton.Text = "Set...";
      this.setMutationButton.UseVisualStyleBackColor = true;
      this.setMutationButton.Click += new System.EventHandler(this.setMutationButton_Click);
      // 
      // setSolutionGenerationButton
      // 
      this.setSolutionGenerationButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.setSolutionGenerationButton.Location = new System.Drawing.Point(419, 380);
      this.setSolutionGenerationButton.Name = "setSolutionGenerationButton";
      this.setSolutionGenerationButton.Size = new System.Drawing.Size(43, 20);
      this.setSolutionGenerationButton.TabIndex = 35;
      this.setSolutionGenerationButton.Text = "Set...";
      this.setSolutionGenerationButton.UseVisualStyleBackColor = true;
      this.setSolutionGenerationButton.Click += new System.EventHandler(this.setSolutionGenerationButton_Click);
      // 
      // viewEvaluationButton
      // 
      this.viewEvaluationButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.viewEvaluationButton.Location = new System.Drawing.Point(360, 432);
      this.viewEvaluationButton.Name = "viewEvaluationButton";
      this.viewEvaluationButton.Size = new System.Drawing.Size(53, 20);
      this.viewEvaluationButton.TabIndex = 42;
      this.viewEvaluationButton.Text = "View...";
      this.viewEvaluationButton.UseVisualStyleBackColor = true;
      this.viewEvaluationButton.Click += new System.EventHandler(this.viewEvaluationButton_Click);
      // 
      // viewMutationButton
      // 
      this.viewMutationButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.viewMutationButton.Location = new System.Drawing.Point(360, 406);
      this.viewMutationButton.Name = "viewMutationButton";
      this.viewMutationButton.Size = new System.Drawing.Size(53, 20);
      this.viewMutationButton.TabIndex = 38;
      this.viewMutationButton.Text = "View...";
      this.viewMutationButton.UseVisualStyleBackColor = true;
      this.viewMutationButton.Click += new System.EventHandler(this.viewMutationButton_Click);
      // 
      // viewSolutionGenerationButton
      // 
      this.viewSolutionGenerationButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.viewSolutionGenerationButton.Location = new System.Drawing.Point(360, 380);
      this.viewSolutionGenerationButton.Name = "viewSolutionGenerationButton";
      this.viewSolutionGenerationButton.Size = new System.Drawing.Size(53, 20);
      this.viewSolutionGenerationButton.TabIndex = 34;
      this.viewSolutionGenerationButton.Text = "View...";
      this.viewSolutionGenerationButton.UseVisualStyleBackColor = true;
      this.viewSolutionGenerationButton.Click += new System.EventHandler(this.viewSolutionGenerationButton_Click);
      // 
      // viewProblemInitializationButton
      // 
      this.viewProblemInitializationButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.viewProblemInitializationButton.Location = new System.Drawing.Point(360, 354);
      this.viewProblemInitializationButton.Name = "viewProblemInitializationButton";
      this.viewProblemInitializationButton.Size = new System.Drawing.Size(53, 20);
      this.viewProblemInitializationButton.TabIndex = 30;
      this.viewProblemInitializationButton.Text = "View...";
      this.viewProblemInitializationButton.UseVisualStyleBackColor = true;
      this.viewProblemInitializationButton.Click += new System.EventHandler(this.viewProblemInitializationButton_Click);
      // 
      // setProblemInitializationButton
      // 
      this.setProblemInitializationButton.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.setProblemInitializationButton.Location = new System.Drawing.Point(419, 354);
      this.setProblemInitializationButton.Name = "setProblemInitializationButton";
      this.setProblemInitializationButton.Size = new System.Drawing.Size(43, 20);
      this.setProblemInitializationButton.TabIndex = 31;
      this.setProblemInitializationButton.Text = "Set...";
      this.setProblemInitializationButton.UseVisualStyleBackColor = true;
      this.setProblemInitializationButton.Click += new System.EventHandler(this.setProblemInitializationButton_Click);
      // 
      // evaluationTextBox
      // 
      this.evaluationTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.evaluationTextBox.Location = new System.Drawing.Point(168, 432);
      this.evaluationTextBox.Name = "evaluationTextBox";
      this.evaluationTextBox.ReadOnly = true;
      this.evaluationTextBox.Size = new System.Drawing.Size(186, 20);
      this.evaluationTextBox.TabIndex = 41;
      // 
      // mutationTextBox
      // 
      this.mutationTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.mutationTextBox.Location = new System.Drawing.Point(168, 406);
      this.mutationTextBox.Name = "mutationTextBox";
      this.mutationTextBox.ReadOnly = true;
      this.mutationTextBox.Size = new System.Drawing.Size(186, 20);
      this.mutationTextBox.TabIndex = 37;
      // 
      // solutionGenerationTextBox
      // 
      this.solutionGenerationTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.solutionGenerationTextBox.Location = new System.Drawing.Point(168, 380);
      this.solutionGenerationTextBox.Name = "solutionGenerationTextBox";
      this.solutionGenerationTextBox.ReadOnly = true;
      this.solutionGenerationTextBox.Size = new System.Drawing.Size(186, 20);
      this.solutionGenerationTextBox.TabIndex = 33;
      // 
      // problemInitializationTextBox
      // 
      this.problemInitializationTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.problemInitializationTextBox.Location = new System.Drawing.Point(168, 354);
      this.problemInitializationTextBox.Name = "problemInitializationTextBox";
      this.problemInitializationTextBox.ReadOnly = true;
      this.problemInitializationTextBox.Size = new System.Drawing.Size(186, 20);
      this.problemInitializationTextBox.TabIndex = 29;
      // 
      // setRandomSeedRandomlyCheckBox
      // 
      this.setRandomSeedRandomlyCheckBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.setRandomSeedRandomlyCheckBox.AutoSize = true;
      this.setRandomSeedRandomlyCheckBox.Location = new System.Drawing.Point(168, 12);
      this.setRandomSeedRandomlyCheckBox.Name = "setRandomSeedRandomlyCheckBox";
      this.setRandomSeedRandomlyCheckBox.Size = new System.Drawing.Size(15, 14);
      this.setRandomSeedRandomlyCheckBox.TabIndex = 1;
      this.setRandomSeedRandomlyCheckBox.UseVisualStyleBackColor = true;
      // 
      // problemDimensionTextBox
      // 
      this.problemDimensionTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.problemDimensionTextBox.Location = new System.Drawing.Point(168, 198);
      this.problemDimensionTextBox.Name = "problemDimensionTextBox";
      this.problemDimensionTextBox.Size = new System.Drawing.Size(186, 20);
      this.problemDimensionTextBox.TabIndex = 24;
      this.problemDimensionTextBox.Validated += new System.EventHandler(this.problemDimensionTextBox_Validated);
      // 
      // shakingFactorsLowerBoundTextBox
      // 
      this.shakingFactorsLowerBoundTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.shakingFactorsLowerBoundTextBox.Location = new System.Drawing.Point(181, 172);
      this.shakingFactorsLowerBoundTextBox.Name = "shakingFactorsLowerBoundTextBox";
      this.shakingFactorsLowerBoundTextBox.Size = new System.Drawing.Size(65, 20);
      this.shakingFactorsLowerBoundTextBox.TabIndex = 18;
      this.toolTip.SetToolTip(this.shakingFactorsLowerBoundTextBox, resources.GetString("shakingFactorsLowerBoundTextBox.ToolTip"));
      // 
      // evaluationLabel
      // 
      this.evaluationLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.evaluationLabel.AutoSize = true;
      this.evaluationLabel.Location = new System.Drawing.Point(6, 435);
      this.evaluationLabel.Name = "evaluationLabel";
      this.evaluationLabel.Size = new System.Drawing.Size(60, 13);
      this.evaluationLabel.TabIndex = 40;
      this.evaluationLabel.Text = "&Evaluation:";
      // 
      // mutationLabel
      // 
      this.mutationLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.mutationLabel.AutoSize = true;
      this.mutationLabel.Location = new System.Drawing.Point(6, 409);
      this.mutationLabel.Name = "mutationLabel";
      this.mutationLabel.Size = new System.Drawing.Size(51, 13);
      this.mutationLabel.TabIndex = 36;
      this.mutationLabel.Text = "&Mutation:";
      // 
      // solutionGenerationLabel
      // 
      this.solutionGenerationLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.solutionGenerationLabel.AutoSize = true;
      this.solutionGenerationLabel.Location = new System.Drawing.Point(6, 383);
      this.solutionGenerationLabel.Name = "solutionGenerationLabel";
      this.solutionGenerationLabel.Size = new System.Drawing.Size(103, 13);
      this.solutionGenerationLabel.TabIndex = 32;
      this.solutionGenerationLabel.Text = "&Solution Generation:";
      // 
      // problemInitializationLabel
      // 
      this.problemInitializationLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.problemInitializationLabel.AutoSize = true;
      this.problemInitializationLabel.Location = new System.Drawing.Point(6, 357);
      this.problemInitializationLabel.Name = "problemInitializationLabel";
      this.problemInitializationLabel.Size = new System.Drawing.Size(105, 13);
      this.problemInitializationLabel.TabIndex = 28;
      this.problemInitializationLabel.Text = "&Problem Initialization:";
      // 
      // problemDimensionLabel
      // 
      this.problemDimensionLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.problemDimensionLabel.AutoSize = true;
      this.problemDimensionLabel.Location = new System.Drawing.Point(6, 201);
      this.problemDimensionLabel.Name = "problemDimensionLabel";
      this.problemDimensionLabel.Size = new System.Drawing.Size(100, 13);
      this.problemDimensionLabel.TabIndex = 23;
      this.problemDimensionLabel.Text = "Problem Dimension:";
      // 
      // mutationStrengthLabel
      // 
      this.mutationStrengthLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.mutationStrengthLabel.AutoSize = true;
      this.mutationStrengthLabel.Location = new System.Drawing.Point(6, 175);
      this.mutationStrengthLabel.Name = "mutationStrengthLabel";
      this.mutationStrengthLabel.Size = new System.Drawing.Size(128, 13);
      this.mutationStrengthLabel.TabIndex = 16;
      this.mutationStrengthLabel.Text = "Mutation Strength Vector:";
      // 
      // mutationRateLabel
      // 
      this.mutationRateLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.mutationRateLabel.AutoSize = true;
      this.mutationRateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.mutationRateLabel.Location = new System.Drawing.Point(6, 123);
      this.mutationRateLabel.Name = "mutationRateLabel";
      this.mutationRateLabel.Size = new System.Drawing.Size(62, 13);
      this.mutationRateLabel.TabIndex = 10;
      this.mutationRateLabel.Text = "Lambda (λ):";
      // 
      // maximumGenerationsTextBox
      // 
      this.maximumGenerationsTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.maximumGenerationsTextBox.Location = new System.Drawing.Point(168, 146);
      this.maximumGenerationsTextBox.Name = "maximumGenerationsTextBox";
      this.maximumGenerationsTextBox.Size = new System.Drawing.Size(186, 20);
      this.maximumGenerationsTextBox.TabIndex = 14;
      // 
      // maximumGenerationsLabel
      // 
      this.maximumGenerationsLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.maximumGenerationsLabel.AutoSize = true;
      this.maximumGenerationsLabel.Location = new System.Drawing.Point(6, 149);
      this.maximumGenerationsLabel.Name = "maximumGenerationsLabel";
      this.maximumGenerationsLabel.Size = new System.Drawing.Size(114, 13);
      this.maximumGenerationsLabel.TabIndex = 13;
      this.maximumGenerationsLabel.Text = "Maximum &Generations:";
      // 
      // randomSeedTextBox
      // 
      this.randomSeedTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.randomSeedTextBox.Location = new System.Drawing.Point(168, 32);
      this.randomSeedTextBox.Name = "randomSeedTextBox";
      this.randomSeedTextBox.Size = new System.Drawing.Size(186, 20);
      this.randomSeedTextBox.TabIndex = 3;
      // 
      // muTextBox
      // 
      this.muTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.muTextBox.Location = new System.Drawing.Point(168, 68);
      this.muTextBox.Name = "muTextBox";
      this.muTextBox.Size = new System.Drawing.Size(186, 20);
      this.muTextBox.TabIndex = 5;
      // 
      // setRandomSeedRandomlyLabel
      // 
      this.setRandomSeedRandomlyLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.setRandomSeedRandomlyLabel.AutoSize = true;
      this.setRandomSeedRandomlyLabel.Location = new System.Drawing.Point(6, 12);
      this.setRandomSeedRandomlyLabel.Name = "setRandomSeedRandomlyLabel";
      this.setRandomSeedRandomlyLabel.Size = new System.Drawing.Size(147, 13);
      this.setRandomSeedRandomlyLabel.TabIndex = 0;
      this.setRandomSeedRandomlyLabel.Text = "Set &Random Seed Randomly:";
      // 
      // randomSeedLabel
      // 
      this.randomSeedLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.randomSeedLabel.AutoSize = true;
      this.randomSeedLabel.Location = new System.Drawing.Point(6, 35);
      this.randomSeedLabel.Name = "randomSeedLabel";
      this.randomSeedLabel.Size = new System.Drawing.Size(78, 13);
      this.randomSeedLabel.TabIndex = 2;
      this.randomSeedLabel.Text = "&Random Seed:";
      // 
      // problemDimensionVariableNameLabel
      // 
      this.problemDimensionVariableNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.problemDimensionVariableNameLabel.AutoSize = true;
      this.problemDimensionVariableNameLabel.Location = new System.Drawing.Point(357, 201);
      this.problemDimensionVariableNameLabel.Name = "problemDimensionVariableNameLabel";
      this.problemDimensionVariableNameLabel.Size = new System.Drawing.Size(100, 13);
      this.problemDimensionVariableNameLabel.TabIndex = 25;
      this.problemDimensionVariableNameLabel.Text = "(ProblemDimension)";
      // 
      // shakingFactorsVariableNameLabel
      // 
      this.shakingFactorsVariableNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.shakingFactorsVariableNameLabel.AutoSize = true;
      this.shakingFactorsVariableNameLabel.Location = new System.Drawing.Point(357, 175);
      this.shakingFactorsVariableNameLabel.Name = "shakingFactorsVariableNameLabel";
      this.shakingFactorsVariableNameLabel.Size = new System.Drawing.Size(87, 13);
      this.shakingFactorsVariableNameLabel.TabIndex = 22;
      this.shakingFactorsVariableNameLabel.Text = "(ShakingFactors)";
      // 
      // maximumGenerationsVariableNameLabel
      // 
      this.maximumGenerationsVariableNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.maximumGenerationsVariableNameLabel.AutoSize = true;
      this.maximumGenerationsVariableNameLabel.Location = new System.Drawing.Point(357, 149);
      this.maximumGenerationsVariableNameLabel.Name = "maximumGenerationsVariableNameLabel";
      this.maximumGenerationsVariableNameLabel.Size = new System.Drawing.Size(114, 13);
      this.maximumGenerationsVariableNameLabel.TabIndex = 15;
      this.maximumGenerationsVariableNameLabel.Text = "(MaximumGenerations)";
      // 
      // esLambdaVariableNameLabel
      // 
      this.esLambdaVariableNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.esLambdaVariableNameLabel.AutoSize = true;
      this.esLambdaVariableNameLabel.Location = new System.Drawing.Point(357, 123);
      this.esLambdaVariableNameLabel.Name = "esLambdaVariableNameLabel";
      this.esLambdaVariableNameLabel.Size = new System.Drawing.Size(61, 13);
      this.esLambdaVariableNameLabel.TabIndex = 12;
      this.esLambdaVariableNameLabel.Text = "(ESlambda)";
      // 
      // esRhoVariableNameLabel
      // 
      this.esRhoVariableNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.esRhoVariableNameLabel.AutoSize = true;
      this.esRhoVariableNameLabel.Location = new System.Drawing.Point(357, 97);
      this.esRhoVariableNameLabel.Name = "esRhoVariableNameLabel";
      this.esRhoVariableNameLabel.Size = new System.Drawing.Size(42, 13);
      this.esRhoVariableNameLabel.TabIndex = 9;
      this.esRhoVariableNameLabel.Text = "(ESrho)";
      // 
      // esMuVariableNameLabel
      // 
      this.esMuVariableNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.esMuVariableNameLabel.AutoSize = true;
      this.esMuVariableNameLabel.Location = new System.Drawing.Point(357, 71);
      this.esMuVariableNameLabel.Name = "esMuVariableNameLabel";
      this.esMuVariableNameLabel.Size = new System.Drawing.Size(41, 13);
      this.esMuVariableNameLabel.TabIndex = 6;
      this.esMuVariableNameLabel.Text = "(ESmu)";
      // 
      // populationSizeLabel
      // 
      this.populationSizeLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.populationSizeLabel.AutoSize = true;
      this.populationSizeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.populationSizeLabel.Location = new System.Drawing.Point(6, 71);
      this.populationSizeLabel.Name = "populationSizeLabel";
      this.populationSizeLabel.Size = new System.Drawing.Size(40, 13);
      this.populationSizeLabel.TabIndex = 4;
      this.populationSizeLabel.Text = "Mu (µ):";
      // 
      // lambdaTextBox
      // 
      this.lambdaTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.lambdaTextBox.Location = new System.Drawing.Point(168, 120);
      this.lambdaTextBox.Name = "lambdaTextBox";
      this.lambdaTextBox.Size = new System.Drawing.Size(186, 20);
      this.lambdaTextBox.TabIndex = 11;
      // 
      // scopesTabPage
      // 
      this.scopesTabPage.Controls.Add(this.scopeView);
      this.scopesTabPage.Location = new System.Drawing.Point(4, 22);
      this.scopesTabPage.Name = "scopesTabPage";
      this.scopesTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.scopesTabPage.Size = new System.Drawing.Size(493, 495);
      this.scopesTabPage.TabIndex = 2;
      this.scopesTabPage.Text = "Scopes";
      this.scopesTabPage.UseVisualStyleBackColor = true;
      // 
      // scopeView
      // 
      this.scopeView.Caption = "Scope";
      this.scopeView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.scopeView.Location = new System.Drawing.Point(3, 3);
      this.scopeView.Name = "scopeView";
      this.scopeView.Scope = null;
      this.scopeView.Size = new System.Drawing.Size(487, 489);
      this.scopeView.TabIndex = 0;
      // 
      // abortButton
      // 
      this.abortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.abortButton.Enabled = false;
      this.abortButton.Location = new System.Drawing.Point(81, 527);
      this.abortButton.Name = "abortButton";
      this.abortButton.Size = new System.Drawing.Size(75, 23);
      this.abortButton.TabIndex = 2;
      this.abortButton.Text = "&Abort";
      this.abortButton.UseVisualStyleBackColor = true;
      this.abortButton.Click += new System.EventHandler(this.abortButton_Click);
      // 
      // resetButton
      // 
      this.resetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.resetButton.Location = new System.Drawing.Point(162, 527);
      this.resetButton.Name = "resetButton";
      this.resetButton.Size = new System.Drawing.Size(75, 23);
      this.resetButton.TabIndex = 3;
      this.resetButton.Text = "&Reset";
      this.resetButton.UseVisualStyleBackColor = true;
      this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
      // 
      // cloneEngineButton
      // 
      this.cloneEngineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cloneEngineButton.Location = new System.Drawing.Point(395, 527);
      this.cloneEngineButton.Name = "cloneEngineButton";
      this.cloneEngineButton.Size = new System.Drawing.Size(106, 23);
      this.cloneEngineButton.TabIndex = 4;
      this.cloneEngineButton.Text = "&Clone Engine...";
      this.cloneEngineButton.UseVisualStyleBackColor = true;
      this.cloneEngineButton.Click += new System.EventHandler(this.cloneEngineButton_Click);
      // 
      // toolTip
      // 
      this.toolTip.AutomaticDelay = 200;
      this.toolTip.AutoPopDelay = 10000;
      this.toolTip.InitialDelay = 200;
      this.toolTip.ReshowDelay = 40;
      // 
      // ESEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControl);
      this.Controls.Add(this.cloneEngineButton);
      this.Controls.Add(this.resetButton);
      this.Controls.Add(this.abortButton);
      this.Controls.Add(this.executeButton);
      this.Name = "ESEditor";
      this.Size = new System.Drawing.Size(501, 550);
      this.tabControl.ResumeLayout(false);
      this.parametersTabPage.ResumeLayout(false);
      this.parametersTabPage.PerformLayout();
      this.successRuleGroupBox.ResumeLayout(false);
      this.successRuleGroupBox.PerformLayout();
      this.parentSelectionGroupBox.ResumeLayout(false);
      this.parentSelectionGroupBox.PerformLayout();
      this.scopesTabPage.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button executeButton;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage parametersTabPage;
    private System.Windows.Forms.Button abortButton;
    private System.Windows.Forms.Button resetButton;
    private System.Windows.Forms.TextBox lambdaTextBox;
    private System.Windows.Forms.Label mutationRateLabel;
    private System.Windows.Forms.TextBox muTextBox;
    private System.Windows.Forms.Label populationSizeLabel;
    private System.Windows.Forms.TabPage scopesTabPage;
    private System.Windows.Forms.TextBox maximumGenerationsTextBox;
    private System.Windows.Forms.Label maximumGenerationsLabel;
    private System.Windows.Forms.TextBox shakingFactorsLowerBoundTextBox;
    private System.Windows.Forms.Label mutationStrengthLabel;
    private System.Windows.Forms.TextBox randomSeedTextBox;
    private System.Windows.Forms.Label setRandomSeedRandomlyLabel;
    private System.Windows.Forms.Label randomSeedLabel;
    private System.Windows.Forms.CheckBox setRandomSeedRandomlyCheckBox;
    private System.Windows.Forms.Label problemInitializationLabel;
    private System.Windows.Forms.Label evaluationLabel;
    private System.Windows.Forms.Label mutationLabel;
    private System.Windows.Forms.Label solutionGenerationLabel;
    private System.Windows.Forms.Button cloneEngineButton;
    private System.Windows.Forms.TextBox mutationTextBox;
    private System.Windows.Forms.TextBox solutionGenerationTextBox;
    private System.Windows.Forms.TextBox problemInitializationTextBox;
    private System.Windows.Forms.TextBox evaluationTextBox;
    private System.Windows.Forms.Button setProblemInitializationButton;
    private System.Windows.Forms.Button setEvaluationButton;
    private System.Windows.Forms.Button setMutationButton;
    private System.Windows.Forms.Button setSolutionGenerationButton;
    private HeuristicLab.Core.ScopeView scopeView;
    private System.Windows.Forms.Button viewEvaluationButton;
    private System.Windows.Forms.Button viewMutationButton;
    private System.Windows.Forms.Button viewSolutionGenerationButton;
    private System.Windows.Forms.Button viewProblemInitializationButton;
    private System.Windows.Forms.TextBox generalLearningRateTextBox;
    private System.Windows.Forms.Label generalLearningRateLabel;
    private System.Windows.Forms.Button setRecombinationButton;
    private System.Windows.Forms.Button viewRecombinationButton;
    private System.Windows.Forms.TextBox recombinationTextBox;
    private System.Windows.Forms.Label recombinationLabel;
    private System.Windows.Forms.TextBox rhoTextBox;
    private System.Windows.Forms.Label rhoLabel;
    private System.Windows.Forms.GroupBox parentSelectionGroupBox;
    private System.Windows.Forms.RadioButton commaRadioButton;
    private System.Windows.Forms.RadioButton plusRadioButton;
    private System.Windows.Forms.GroupBox successRuleGroupBox;
    private System.Windows.Forms.TextBox learningRateTextBox;
    private System.Windows.Forms.Label learningRateLabel;
    private System.Windows.Forms.Label maximumGenerationsVariableNameLabel;
    private System.Windows.Forms.Label esLambdaVariableNameLabel;
    private System.Windows.Forms.Label esRhoVariableNameLabel;
    private System.Windows.Forms.Label esMuVariableNameLabel;
    private System.Windows.Forms.Label shakingFactorsVariableNameLabel;
    private System.Windows.Forms.Label learningRateVariableNameLabel;
    private System.Windows.Forms.Label generalLearningRateVariableNameLabel;
    private System.Windows.Forms.ToolTip toolTip;
    private System.Windows.Forms.TextBox problemDimensionTextBox;
    private System.Windows.Forms.Label problemDimensionLabel;
    private System.Windows.Forms.Label leftBracketLabelRight;
    private System.Windows.Forms.Label semicolonLabel;
    private System.Windows.Forms.Label leftBracketLabelLeft;
    private System.Windows.Forms.TextBox shakingFactorsUpperBoundTextBox;
    private System.Windows.Forms.Label problemDimensionVariableNameLabel;
  }
}
