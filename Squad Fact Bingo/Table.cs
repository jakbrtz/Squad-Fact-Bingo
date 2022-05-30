using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Squad_Fact_Bingo
{
    class Table
    {
        internal string[] facts;
        internal int[] counts;
        internal string[][] names;
    }

    class Guess
    {
        internal Table table = new Table();
        internal Feedback[][] feedback;

        internal Guess Clone()
        {
            Guess guess = new Guess()
            {
                table = table,
                feedback = new Feedback[feedback.Length][],
            };
            for (int i = 0; i < feedback.Length; i++)
            {
                guess.feedback[i] = new Feedback[feedback[i].Length];
                for (int j = 0; j < feedback[i].Length; j++)
                {
                    guess.feedback[i][j] = feedback[i][j];
                }
            }
            return guess;
        }

        internal void GiveFeedback(int guessIndex, int factIndex, Feedback result)
        {
            if (guessIndex >= 0 && guessIndex < table.names.Length && factIndex >= 0 && factIndex < table.facts.Length)
            {
                AddFeedback(guessIndex, factIndex, result);
                TryForceFeedback();
            }
        }

        internal void AddFeedback(int guessIndex, int factIndex, Feedback result)
        {
            feedback[guessIndex][factIndex] = feedback[guessIndex][factIndex] == result ? Feedback.Unsure : result;
            Process(guessIndex, factIndex);
        }

        internal void Clear()
        {
            for (int i = 0; i < feedback.Length; i++)
            {
                for (int j = 0; j < feedback[i].Length; j++)
                {
                    feedback[i][j] = Feedback.Unsure;
                }
            }
            TryForceFeedback();
        }

        private void Process(int guessIndex, int feedbackIndex)
        {
            Queue<(int, int)> todo = new Queue<(int, int)>();
            todo.Enqueue((guessIndex, feedbackIndex));
            Process(todo);
        }

        private void Process(Queue<(int, int)> todo)
        {
            void Set(int guessIndex, int factIndex, Feedback result)
            {
                if (feedback[guessIndex][factIndex] == Feedback.Unsure)
                {
                    feedback[guessIndex][factIndex] = result;
                    todo.Enqueue((guessIndex, factIndex));
                }
                else if (feedback[guessIndex][factIndex] != result)
                {

                }
            }

            while (todo.Any())
            {
                (int guessIndex, int factIndex) = todo.Dequeue();
                Feedback result = feedback[guessIndex][factIndex];
                if (result == Feedback.No)
                {
                    for (int i = 0; i < feedback.Length; i++)
                    {
                        if (table.names[i][factIndex] == table.names[guessIndex][factIndex])
                        {
                            feedback[i][factIndex] = Feedback.No;
                        }
                    }

                    if (table.counts[guessIndex] != Constants.NoNum)
                    {
                        int count = 0;
                        for (int j = 0; j < table.facts.Length; j++)
                        {
                            if (feedback[guessIndex][j] == Feedback.No)
                            {
                                count++;
                            }
                        }
                        if (count == table.facts.Length - table.counts[guessIndex])
                        {
                            for (int j = 0; j < table.facts.Length; j++)
                            {
                                if (feedback[guessIndex][j] == Feedback.Unsure)
                                {
                                    Set(guessIndex, j, Feedback.Yes);
                                }
                            }
                        }
                    }
                }
                else if (result == Feedback.Yes)
                {
                    for (int i = 0; i < feedback.Length; i++)
                    {
                        Set(i, factIndex, table.names[i][factIndex] == table.names[guessIndex][factIndex] ? Feedback.Yes : Feedback.No);
                        for (int j = 0; j < feedback[i].Length; j++)
                        {
                            if (j != factIndex && table.names[i][j] == table.names[guessIndex][factIndex])
                            {
                                Set(i, j, Feedback.No);
                            }
                        }
                    }
                    if (table.counts[guessIndex] != Constants.NoNum)
                    {
                        int count = 0;
                        for (int j = 0; j < table.facts.Length; j++)
                        {
                            if (feedback[guessIndex][j] == Feedback.Yes)
                            {
                                count++;
                            }
                        }
                        if (count == table.counts[guessIndex])
                        {
                            for (int j = 0; j < table.facts.Length; j++)
                            {
                                if (feedback[guessIndex][j] == Feedback.Unsure)
                                {
                                    Set(guessIndex, j, Feedback.No);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool IsValid()
        {
            string[] answers = new string[table.facts.Length];
            // only one person per fact
            for (int factIndex = 0; factIndex < table.facts.Length; factIndex++)
            {
                string answer = null;
                for (int guessIndex = 0; guessIndex < table.names.Length; guessIndex++)
                {
                    if (feedback[guessIndex][factIndex] == Feedback.Yes)
                    {
                        answer = table.names[guessIndex][factIndex];
                        break;
                    }
                }
                if (answer == null)
                {
                    continue;
                }
                answers[factIndex] = answer;
                for (int guessIndex = 0; guessIndex < table.names.Length; guessIndex++)
                {
                    if (feedback[guessIndex][factIndex] != (answer == table.names[guessIndex][factIndex] ? Feedback.Yes : Feedback.No))
                    {
                        return false;
                    }
                }
            }
            // only one fact per person
            for (int i = 0; i < answers.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (answers[i] != null && answers[i] == answers[j])
                    {
                        return false;
                    }
                }
            }
            // correct count per guess
            for (int guessIndex = 0; guessIndex < feedback.Length; guessIndex++)
            {
                if (table.counts[guessIndex] == Constants.NoNum)
                {
                    continue;
                }
                int countYes = 0;
                int countUnsure = 0;
                for (int factIndex = 0; factIndex < feedback[guessIndex].Length; factIndex++)
                {
                    if (feedback[guessIndex][factIndex] == Feedback.Yes)
                    {
                        countYes++;
                    }
                    else if (feedback[guessIndex][factIndex] == Feedback.Unsure)
                    {
                        countUnsure++;
                    }
                }
                if (table.counts[guessIndex] < countYes)
                {
                    return false;
                }
                if (countYes + countUnsure < table.counts[guessIndex])
                {
                    return false;
                }
            }
            // No problems found
            return true;
        }

        private bool FeedbackLeadsToFail(int guessIndex, int factIndex, Feedback result)
        {
            Debug.Assert(this.IsValid());
            Guess clone = this.Clone();
            clone.AddFeedback(guessIndex, factIndex, result);
            return !clone.IsValid();
        }
        private bool TryForceFeedback(int guessIndex, int factIndex)
        {
            Debug.Assert(this.IsValid());
            if (feedback[guessIndex][factIndex] != Feedback.Unsure) return false;

            if (FeedbackLeadsToFail(guessIndex, factIndex, Feedback.Yes))
            {
                AddFeedback(guessIndex, factIndex, Feedback.No);
                Debug.Assert(this.IsValid());
                return true;
            }

            if (FeedbackLeadsToFail(guessIndex, factIndex, Feedback.No))
            {
                AddFeedback(guessIndex, factIndex, Feedback.Yes);
                Debug.Assert(this.IsValid());
                return true;
            }

            return false;
        }

        private void TryForceFeedback()
        {
            while (true)
            {
                bool somethingDone = false;
                for (int guessIndex = 0; guessIndex < table.names.Length; guessIndex++)
                {
                    for (int factIndex = 0; factIndex < table.names[guessIndex].Length; factIndex++)
                    {
                        if (TryForceFeedback(guessIndex, factIndex))
                        {
                            somethingDone = true;
                        }
                    }
                }
                if (!somethingDone)
                {
                    break;
                }
            }
        }

        internal (string, string)[] MakeGuess()
        {
            Node[] factNodes = new Node[table.facts.Length];
            Node[] nameNodes = new Node[table.facts.Length];
            for (int i = 0; i < table.facts.Length; i++)
            {
                factNodes[i] = new Node() { title = table.facts[i] };
                nameNodes[i] = new Node() { title = table.names[0][i] };
            }
            Node findName(string name)
            {
                for (int i = 0; i < nameNodes.Length; i++)
                {
                    if (nameNodes[i].title == name)
                    {
                        return nameNodes[i];
                    }
                }
                throw new FormatException();
            }
            for (int factIndex = 0; factIndex < factNodes.Length; factIndex++)
            {
                for (int guessIndex = 0; guessIndex < feedback.Length; guessIndex++)
                {
                    if (feedback[guessIndex][factIndex] == Feedback.Yes)
                    {
                        factNodes[factIndex].options.Add(findName(table.names[guessIndex][factIndex]));
                        break;
                    }
                }
                if (factNodes[factIndex].options.Count == 0)
                {
                    for (int nameIndex = 0; nameIndex < nameNodes.Length; nameIndex++)
                    {
                        bool valid = true;
                        for (int guessIndex = 0; guessIndex < feedback.Length; guessIndex++)
                        {
                            if (feedback[guessIndex][factIndex] == Feedback.No && nameNodes[nameIndex].title == table.names[guessIndex][factIndex])
                            {
                                valid = false;
                                break;
                            }
                        }
                        if (valid)
                        {
                            factNodes[factIndex].options.Add(nameNodes[nameIndex]);
                        }
                    }
                }
            }
            for (int i = 0; i < factNodes.Length; i++)
            {
                for (int j = 0; j < factNodes[i].options.Count; j++)
                {
                    factNodes[i].options[j].options.Add(factNodes[i]);
                }
            }
            // Nodes are finally set up, time for bipartite matching
            static bool TryMatch(Node node)
            {
                static void Link(Node node1, Node node2)
                {
                    Debug.Assert(node1.chosen == null);
                    Debug.Assert(node2.chosen == null);
                    node1.chosen = node2;
                    node2.chosen = node1;
                    node1.forbidden.Remove(node2);
                    node2.forbidden.Remove(node1);
                }
                static void Unlink(Node node1, Node node2)
                {
                    Debug.Assert(node1.chosen == node2);
                    Debug.Assert(node2.chosen == node1);
                    node1.chosen = null;
                    node2.chosen = null;
                    node1.forbidden.Add(node2);
                    node2.forbidden.Add(node1);
                }
                foreach (Node option in node.options)
                {
                    if (option.chosen == null)
                    {
                        Link(node, option);
                        return true;
                    }
                }
                foreach (Node option in node.options)
                {
                    if (node.forbidden.Contains(option))
                    {
                        continue;
                    }
                    Node oldChosen = option.chosen;
                    Unlink(option, oldChosen);
                    Link(node, option);
                    if (TryMatch(oldChosen))
                    {
                        return true;
                    }
                    Unlink(node, option);
                    Link(option, oldChosen);
                }
                return false;
            }
            for (int i = 0; i < factNodes.Length; i++)
            {
                for (int j = 0; j < factNodes.Length; j++)
                {
                    factNodes[j].forbidden.Clear();
                    nameNodes[j].forbidden.Clear();
                }
                bool success = TryMatch(factNodes[i]);
                Debug.Assert(success);
            }
            (string, string)[] output = new (string, string)[factNodes.Length];
            for (int i = 0; i < factNodes.Length; i++)
            {
                output[i] = (factNodes[i].title, factNodes[i].chosen.title);
            }
            return output;
        }

    }

    enum Feedback { Unsure, Yes, No }

    class Node
    {
        internal string title;
        internal List<Node> options = new List<Node>();
        internal Node? chosen;
        internal List<Node> forbidden = new List<Node>();
    }
}
