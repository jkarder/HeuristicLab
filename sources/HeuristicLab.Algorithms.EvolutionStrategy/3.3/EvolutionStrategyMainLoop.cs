#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2010 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

using HeuristicLab.Analysis;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Operators;
using HeuristicLab.Optimization.Operators;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Selection;
using HeuristicLab.Encodings.RealVectorEncoding;

namespace HeuristicLab.Algorithms.EvolutionStrategy {
  /// <summary>
  /// An operator which represents the main loop of an evolution strategy (EvolutionStrategy).
  /// </summary>
  [Item("EvolutionStrategyMainLoop", "An operator which represents the main loop of an evolution strategy (EvolutionStrategy).")]
  [StorableClass]
  public sealed class EvolutionStrategyMainLoop : AlgorithmOperator {
    #region Parameter properties
    public ValueLookupParameter<IRandom> RandomParameter {
      get { return (ValueLookupParameter<IRandom>)Parameters["Random"]; }
    }
    public ValueLookupParameter<BoolValue> MaximizationParameter {
      get { return (ValueLookupParameter<BoolValue>)Parameters["Maximization"]; }
    }
    public SubScopesLookupParameter<DoubleValue> QualityParameter {
      get { return (SubScopesLookupParameter<DoubleValue>)Parameters["Quality"]; }
    }
    public ValueLookupParameter<DoubleValue> BestKnownQualityParameter {
      get { return (ValueLookupParameter<DoubleValue>)Parameters["BestKnownQuality"]; }
    }
    public ValueLookupParameter<IntValue> PopulationSizeParameter {
      get { return (ValueLookupParameter<IntValue>)Parameters["PopulationSize"]; }
    }
    public ValueLookupParameter<IntValue> ParentsPerChildParameter {
      get { return (ValueLookupParameter<IntValue>)Parameters["ParentsPerChild"]; }
    }
    public ValueLookupParameter<IntValue> ChildrenParameter {
      get { return (ValueLookupParameter<IntValue>)Parameters["Children"]; }
    }
    public ValueLookupParameter<RealVector> StrategyVectorParameter {
      get { return (ValueLookupParameter<RealVector>)Parameters["StrategyVector"]; }
    }
    public ValueLookupParameter<DoubleMatrix> StrategyVectorBoundsParameter {
      get { return (ValueLookupParameter<DoubleMatrix>)Parameters["StrategyVectorBounds"]; }
    }
    public ValueLookupParameter<DoubleValue> GeneralLearningRateParameter {
      get { return (ValueLookupParameter<DoubleValue>)Parameters["GeneralLearningRate"]; }
    }
    public ValueLookupParameter<DoubleValue> LearningRateParameter {
      get { return (ValueLookupParameter<DoubleValue>)Parameters["LearningRate"]; }
    }
    public ValueLookupParameter<BoolValue> PlusSelectionParameter {
      get { return (ValueLookupParameter<BoolValue>)Parameters["PlusSelection"]; }
    }
    public ValueLookupParameter<IntValue> MaximumGenerationsParameter {
      get { return (ValueLookupParameter<IntValue>)Parameters["MaximumGenerations"]; }
    }
    public ValueLookupParameter<IOperator> MutatorParameter {
      get { return (ValueLookupParameter<IOperator>)Parameters["Mutator"]; }
    }
    public ValueLookupParameter<IOperator> RecombinatorParameter {
      get { return (ValueLookupParameter<IOperator>)Parameters["Recombinator"]; }
    }
    public ValueLookupParameter<IOperator> EvaluatorParameter {
      get { return (ValueLookupParameter<IOperator>)Parameters["Evaluator"]; }
    }
    public ValueLookupParameter<VariableCollection> ResultsParameter {
      get { return (ValueLookupParameter<VariableCollection>)Parameters["Results"]; }
    }
    public ValueLookupParameter<IOperator> VisualizerParameter {
      get { return (ValueLookupParameter<IOperator>)Parameters["Visualizer"]; }
    }
    public LookupParameter<IItem> VisualizationParameter {
      get { return (LookupParameter<IItem>)Parameters["Visualization"]; }
    }
    private ScopeParameter CurrentScopeParameter {
      get { return (ScopeParameter)Parameters["CurrentScope"]; }
    }

    public IScope CurrentScope {
      get { return CurrentScopeParameter.ActualValue; }
    }
    #endregion

    [StorableConstructor]
    private EvolutionStrategyMainLoop(bool deserializing) : base() { }
    public EvolutionStrategyMainLoop()
      : base() {
      Initialize();
    }

    private void Initialize() {
      #region Create parameters
      Parameters.Add(new ValueLookupParameter<IRandom>("Random", "A pseudo random number generator."));
      Parameters.Add(new ValueLookupParameter<BoolValue>("Maximization", "True if the problem is a maximization problem, otherwise false."));
      Parameters.Add(new SubScopesLookupParameter<DoubleValue>("Quality", "The value which represents the quality of a solution."));
      Parameters.Add(new ValueLookupParameter<DoubleValue>("BestKnownQuality", "The best known quality value found so far."));
      Parameters.Add(new ValueLookupParameter<IntValue>("PopulationSize", "µ (mu) - the size of the population."));
      Parameters.Add(new ValueLookupParameter<IntValue>("ParentsPerChild", "ρ (rho) - how many parents should be recombined."));
      Parameters.Add(new ValueLookupParameter<IntValue>("Children", "λ (lambda) - the size of the offspring population."));
      Parameters.Add(new ValueLookupParameter<IntValue>("MaximumGenerations", "The maximum number of generations which should be processed."));
      Parameters.Add(new ValueLookupParameter<RealVector>("StrategyVector", "The strategy vector."));
      Parameters.Add(new LookupParameter<DoubleMatrix>("StrategyVectorBounds", "2 column matrix with one row for each dimension specifying upper and lower bound for the strategy vector. If there are less rows than dimensions, the strategy vector will be read in a cycle."));
      Parameters.Add(new ValueLookupParameter<DoubleValue>("GeneralLearningRate", "τ0 (tau0) - the factor with which adjustments in the strategy vector is dampened over all dimensions. Recommendation is to use 1/Sqrt(2*ProblemDimension)."));
      Parameters.Add(new ValueLookupParameter<DoubleValue>("LearningRate", "τ (tau) - the factor with which adjustments in the strategy vector are dampened in a single dimension. Recommendation is to use 1/Sqrt(2*Sqrt(ProblemDimension))."));
      Parameters.Add(new ValueLookupParameter<BoolValue>("PlusSelection", "True for plus selection (elitist population), false for comma selection (non-elitist population)."));
      Parameters.Add(new ValueLookupParameter<IOperator>("Mutator", "The operator used to mutate solutions."));
      Parameters.Add(new ValueLookupParameter<IOperator>("Recombinator", "The operator used to cross solutions."));
      Parameters.Add(new ValueLookupParameter<IOperator>("Evaluator", "The operator used to evaluate solutions."));
      Parameters.Add(new ValueLookupParameter<VariableCollection>("Results", "The variable collection where results should be stored."));
      Parameters.Add(new ValueLookupParameter<IOperator>("Visualizer", "The operator used to visualize solutions."));
      Parameters.Add(new LookupParameter<IItem>("Visualization", "The item which represents the visualization of solutions."));
      Parameters.Add(new ScopeParameter("CurrentScope", "The current scope which represents a population of solutions on which the EvolutionStrategy should be applied."));
      #endregion

      #region Create operators
      VariableCreator variableCreator = new VariableCreator();
      BestQualityMemorizer bestQualityMemorizer1 = new BestQualityMemorizer();
      BestQualityMemorizer bestQualityMemorizer2 = new BestQualityMemorizer();
      BestAverageWorstQualityCalculator bestAverageWorstQualityCalculator1 = new BestAverageWorstQualityCalculator();
      DataTableValuesCollector dataTableValuesCollector1 = new DataTableValuesCollector();
      QualityDifferenceCalculator qualityDifferenceCalculator1 = new QualityDifferenceCalculator();
      Placeholder visualizer1 = new Placeholder();
      ResultsCollector resultsCollector = new ResultsCollector();
      WithoutRepeatingBatchedRandomSelector selector = new WithoutRepeatingBatchedRandomSelector();
      SubScopesProcessor subScopesProcessor1 = new SubScopesProcessor();
      Comparator useRecombinationComparator = new Comparator();
      ConditionalBranch useRecombinationBranch = new ConditionalBranch();
      ChildrenCreator childrenCreator = new ChildrenCreator();
      UniformSubScopesProcessor uniformSubScopesProcessor1 = new UniformSubScopesProcessor();
      Placeholder recombinator = new Placeholder();
      StrategyVectorManipulator strategyMutator1 = new StrategyVectorManipulator();
      Placeholder mutator1 = new Placeholder();
      Placeholder evaluator1 = new Placeholder();
      SubScopesRemover subScopesRemover = new SubScopesRemover();
      UniformSubScopesProcessor uniformSubScopesProcessor2 = new UniformSubScopesProcessor();
      StrategyVectorManipulator strategyMutator2 = new StrategyVectorManipulator();
      Placeholder mutator2 = new Placeholder();
      Placeholder evaluator2 = new Placeholder();
      ConditionalBranch plusOrCommaReplacementBranch = new ConditionalBranch();
      MergingReducer plusReplacement = new MergingReducer();
      RightReducer commaReplacement = new RightReducer();
      BestSelector bestSelector = new BestSelector();
      RightReducer rightReducer = new RightReducer();
      IntCounter intCounter = new IntCounter();
      Comparator comparator = new Comparator();
      BestQualityMemorizer bestQualityMemorizer3 = new BestQualityMemorizer();
      BestQualityMemorizer bestQualityMemorizer4 = new BestQualityMemorizer();
      BestAverageWorstQualityCalculator bestAverageWorstQualityCalculator2 = new BestAverageWorstQualityCalculator();
      DataTableValuesCollector dataTableValuesCollector2 = new DataTableValuesCollector();
      QualityDifferenceCalculator qualityDifferenceCalculator2 = new QualityDifferenceCalculator();
      Placeholder visualizer2 = new Placeholder();
      ConditionalBranch conditionalBranch = new ConditionalBranch();

      variableCreator.CollectedValues.Add(new ValueParameter<IntValue>("Generations", new IntValue(0)));

      bestQualityMemorizer1.BestQualityParameter.ActualName = "BestQuality";
      bestQualityMemorizer1.MaximizationParameter.ActualName = MaximizationParameter.Name;
      bestQualityMemorizer1.QualityParameter.ActualName = QualityParameter.Name;

      bestQualityMemorizer2.BestQualityParameter.ActualName = BestKnownQualityParameter.Name;
      bestQualityMemorizer2.MaximizationParameter.ActualName = MaximizationParameter.Name;
      bestQualityMemorizer2.QualityParameter.ActualName = QualityParameter.Name;

      bestAverageWorstQualityCalculator1.AverageQualityParameter.ActualName = "CurrentAverageQuality";
      bestAverageWorstQualityCalculator1.BestQualityParameter.ActualName = "CurrentBestQuality";
      bestAverageWorstQualityCalculator1.MaximizationParameter.ActualName = MaximizationParameter.Name;
      bestAverageWorstQualityCalculator1.QualityParameter.ActualName = QualityParameter.Name;
      bestAverageWorstQualityCalculator1.WorstQualityParameter.ActualName = "CurrentWorstQuality";

      dataTableValuesCollector1.CollectedValues.Add(new LookupParameter<DoubleValue>("Current Best Quality", null, "CurrentBestQuality"));
      dataTableValuesCollector1.CollectedValues.Add(new LookupParameter<DoubleValue>("Current Average Quality", null, "CurrentAverageQuality"));
      dataTableValuesCollector1.CollectedValues.Add(new LookupParameter<DoubleValue>("Current Worst Quality", null, "CurrentWorstQuality"));
      dataTableValuesCollector1.CollectedValues.Add(new LookupParameter<DoubleValue>("Best Quality", null, "BestQuality"));
      dataTableValuesCollector1.CollectedValues.Add(new LookupParameter<DoubleValue>("Best Known Quality", null, BestKnownQualityParameter.Name));
      dataTableValuesCollector1.DataTableParameter.ActualName = "Qualities";

      qualityDifferenceCalculator1.AbsoluteDifferenceParameter.ActualName = "AbsoluteDifferenceBestKnownToBest";
      qualityDifferenceCalculator1.FirstQualityParameter.ActualName = BestKnownQualityParameter.Name;
      qualityDifferenceCalculator1.RelativeDifferenceParameter.ActualName = "RelativeDifferenceBestKnownToBest";
      qualityDifferenceCalculator1.SecondQualityParameter.ActualName = "BestQuality";

      visualizer1.Name = "Visualizer (placeholder)";
      visualizer1.OperatorParameter.ActualName = VisualizerParameter.Name;

      resultsCollector.CollectedValues.Add(new LookupParameter<IntValue>("Generations"));
      resultsCollector.CollectedValues.Add(new LookupParameter<DoubleValue>("Current Best Quality", null, "CurrentBestQuality"));
      resultsCollector.CollectedValues.Add(new LookupParameter<DoubleValue>("Current Average Quality", null, "CurrentAverageQuality"));
      resultsCollector.CollectedValues.Add(new LookupParameter<DoubleValue>("Current Worst Quality", null, "CurrentWorstQuality"));
      resultsCollector.CollectedValues.Add(new LookupParameter<DoubleValue>("Best Quality", null, "BestQuality"));
      resultsCollector.CollectedValues.Add(new LookupParameter<DoubleValue>("Best Known Quality", null, BestKnownQualityParameter.Name));
      resultsCollector.CollectedValues.Add(new LookupParameter<DoubleValue>("Absolute Difference of Best Known Quality to Best Quality", null, "AbsoluteDifferenceBestKnownToBest"));
      resultsCollector.CollectedValues.Add(new LookupParameter<DoubleValue>("Relative Difference of Best Known Quality to Best Quality", null, "RelativeDifferenceBestKnownToBest"));
      resultsCollector.CollectedValues.Add(new LookupParameter<IItem>("Solution Visualization", null, VisualizationParameter.Name));
      resultsCollector.CollectedValues.Add(new LookupParameter<DataTable>("Qualities"));
      resultsCollector.ResultsParameter.ActualName = ResultsParameter.Name;

      selector.Name = "ES Random Selector";
      selector.ParentsPerChildParameter.ActualName = ParentsPerChildParameter.Name;
      selector.ChildrenParameter.ActualName = ChildrenParameter.Name;

      useRecombinationComparator.Name = "ParentsPerChild > 1";
      useRecombinationComparator.LeftSideParameter.ActualName = ParentsPerChildParameter.Name;
      useRecombinationComparator.RightSideParameter.Value = new IntValue(1);
      useRecombinationComparator.Comparison = new Comparison(ComparisonType.Greater);
      useRecombinationComparator.ResultParameter.ActualName = "UseRecombination";

      useRecombinationBranch.Name = "Use Recombination?";
      useRecombinationBranch.ConditionParameter.ActualName = "UseRecombination";

      childrenCreator.ParentsPerChildParameter.ActualName = ParentsPerChildParameter.Name;

      recombinator.Name = "Recombinator (placeholder)";
      recombinator.OperatorParameter.ActualName = RecombinatorParameter.Name;

      strategyMutator1.StrategyVectorParameter.ActualName = StrategyVectorParameter.Name;
      strategyMutator1.GeneralLearningRateParameter.ActualName = GeneralLearningRateParameter.Name;
      strategyMutator1.LearningRateParameter.ActualName = LearningRateParameter.Name;
      strategyMutator1.RandomParameter.ActualName = RandomParameter.Name;

      mutator1.Name = "Mutator (placeholder)";
      mutator1.OperatorParameter.ActualName = MutatorParameter.Name;

      evaluator1.Name = "Evaluator (placeholder)";
      evaluator1.OperatorParameter.ActualName = EvaluatorParameter.Name;

      subScopesRemover.RemoveAllSubScopes = true;

      strategyMutator2.StrategyVectorParameter.ActualName = StrategyVectorParameter.Name;
      strategyMutator2.GeneralLearningRateParameter.ActualName = GeneralLearningRateParameter.Name;
      strategyMutator2.LearningRateParameter.ActualName = LearningRateParameter.Name;
      strategyMutator2.RandomParameter.ActualName = RandomParameter.Name;

      mutator2.Name = "Mutator (placeholder)";
      mutator2.OperatorParameter.ActualName = MutatorParameter.Name;

      evaluator2.Name = "Evaluator (placeholder)";
      evaluator2.OperatorParameter.ActualName = EvaluatorParameter.Name;

      plusOrCommaReplacementBranch.ConditionParameter.ActualName = PlusSelectionParameter.Name;

      bestSelector.CopySelected = new BoolValue(false);
      bestSelector.MaximizationParameter.ActualName = MaximizationParameter.Name;
      bestSelector.NumberOfSelectedSubScopesParameter.ActualName = PopulationSizeParameter.Name;
      bestSelector.QualityParameter.ActualName = QualityParameter.Name;

      intCounter.Increment = new IntValue(1);
      intCounter.ValueParameter.ActualName = "Generations";

      comparator.Comparison = new Comparison(ComparisonType.GreaterOrEqual);
      comparator.LeftSideParameter.ActualName = "Generations";
      comparator.ResultParameter.ActualName = "Terminate";
      comparator.RightSideParameter.ActualName = MaximumGenerationsParameter.Name;

      bestQualityMemorizer3.BestQualityParameter.ActualName = "BestQuality";
      bestQualityMemorizer3.MaximizationParameter.ActualName = MaximizationParameter.Name;
      bestQualityMemorizer3.QualityParameter.ActualName = QualityParameter.Name;

      bestQualityMemorizer4.BestQualityParameter.ActualName = BestKnownQualityParameter.Name;
      bestQualityMemorizer4.MaximizationParameter.ActualName = MaximizationParameter.Name;
      bestQualityMemorizer4.QualityParameter.ActualName = QualityParameter.Name;

      bestAverageWorstQualityCalculator2.AverageQualityParameter.ActualName = "CurrentAverageQuality";
      bestAverageWorstQualityCalculator2.BestQualityParameter.ActualName = "CurrentBestQuality";
      bestAverageWorstQualityCalculator2.MaximizationParameter.ActualName = MaximizationParameter.Name;
      bestAverageWorstQualityCalculator2.QualityParameter.ActualName = QualityParameter.Name;
      bestAverageWorstQualityCalculator2.WorstQualityParameter.ActualName = "CurrentWorstQuality";

      dataTableValuesCollector2.CollectedValues.Add(new LookupParameter<DoubleValue>("Current Best Quality", null, "CurrentBestQuality"));
      dataTableValuesCollector2.CollectedValues.Add(new LookupParameter<DoubleValue>("Current Average Quality", null, "CurrentAverageQuality"));
      dataTableValuesCollector2.CollectedValues.Add(new LookupParameter<DoubleValue>("Current Worst Quality", null, "CurrentWorstQuality"));
      dataTableValuesCollector2.CollectedValues.Add(new LookupParameter<DoubleValue>("Best Quality", null, "BestQuality"));
      dataTableValuesCollector2.CollectedValues.Add(new LookupParameter<DoubleValue>("Best Known Quality", null, BestKnownQualityParameter.Name));
      dataTableValuesCollector2.DataTableParameter.ActualName = "Qualities";

      qualityDifferenceCalculator2.AbsoluteDifferenceParameter.ActualName = "AbsoluteDifferenceBestKnownToBest";
      qualityDifferenceCalculator2.FirstQualityParameter.ActualName = BestKnownQualityParameter.Name;
      qualityDifferenceCalculator2.RelativeDifferenceParameter.ActualName = "RelativeDifferenceBestKnownToBest";
      qualityDifferenceCalculator2.SecondQualityParameter.ActualName = "BestQuality";

      visualizer2.Name = "Visualizer (placeholder)";
      visualizer2.OperatorParameter.ActualName = VisualizerParameter.Name;

      conditionalBranch.ConditionParameter.ActualName = "Terminate";
      #endregion

      #region Create operator graph
      OperatorGraph.InitialOperator = variableCreator;
      variableCreator.Successor = bestQualityMemorizer1;
      bestQualityMemorizer1.Successor = bestQualityMemorizer2;
      bestQualityMemorizer2.Successor = bestAverageWorstQualityCalculator1;
      bestAverageWorstQualityCalculator1.Successor = dataTableValuesCollector1;
      dataTableValuesCollector1.Successor = qualityDifferenceCalculator1;
      qualityDifferenceCalculator1.Successor = visualizer1;
      visualizer1.Successor = resultsCollector;
      resultsCollector.Successor = selector;
      selector.Successor = subScopesProcessor1;
      subScopesProcessor1.Operators.Add(new EmptyOperator());
      subScopesProcessor1.Operators.Add(useRecombinationComparator);
      subScopesProcessor1.Successor = plusOrCommaReplacementBranch;
      useRecombinationComparator.Successor = useRecombinationBranch;
      useRecombinationBranch.TrueBranch = childrenCreator;
      useRecombinationBranch.FalseBranch = uniformSubScopesProcessor2;
      useRecombinationBranch.Successor = null;
      childrenCreator.Successor = uniformSubScopesProcessor1;
      uniformSubScopesProcessor1.Operator = recombinator;
      uniformSubScopesProcessor1.Successor = null;
      recombinator.Successor = strategyMutator1;
      strategyMutator1.Successor = mutator1;
      mutator1.Successor = evaluator1;
      evaluator1.Successor = subScopesRemover;
      subScopesRemover.Successor = null;
      uniformSubScopesProcessor2.Operator = strategyMutator2;
      uniformSubScopesProcessor2.Successor = null;
      strategyMutator2.Successor = mutator2;
      mutator2.Successor = evaluator2;
      plusOrCommaReplacementBranch.TrueBranch = plusReplacement;
      plusOrCommaReplacementBranch.FalseBranch = commaReplacement;
      plusOrCommaReplacementBranch.Successor = bestSelector;
      bestSelector.Successor = rightReducer;
      rightReducer.Successor = intCounter;
      intCounter.Successor = comparator;
      comparator.Successor = bestQualityMemorizer3;
      bestQualityMemorizer3.Successor = bestQualityMemorizer4;
      bestQualityMemorizer4.Successor = bestAverageWorstQualityCalculator2;
      bestAverageWorstQualityCalculator2.Successor = dataTableValuesCollector2;
      dataTableValuesCollector2.Successor = qualityDifferenceCalculator2;
      qualityDifferenceCalculator2.Successor = visualizer2;
      visualizer2.Successor = conditionalBranch;
      conditionalBranch.FalseBranch = selector;
      conditionalBranch.TrueBranch = null;
      conditionalBranch.Successor = null;
      #endregion
    }
  }
}
