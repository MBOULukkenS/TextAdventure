using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace TerminalUI
{
    public class Wm2000VariableStorage : VariableStorageBehaviour
    {
        [SerializeField]
        private Dictionary<string, Value> variables = new Dictionary<string, Value>();

        /// A default value to apply when the object wakes up, or
        /// when ResetToDefaults is called
        [System.Serializable]
        public class DefaultVariable
        {
            /// Name of the variable
            public string name;
            /// Value of the variable
            public string value;
            /// Type of the variable
            public Value.Type type;
        }

        /// Our list of default variables, for debugging.
        public DefaultVariable[] defaultVariables;

        /// Reset to our default values when the game starts
        void Awake()
        {
            ResetToDefaults();
        }

        /// Erase all variables and reset to default values
        public override void ResetToDefaults()
        {
            Clear();

            // For each default variable that's been defined, parse the string
            // that the user typed in in Unity and store the variable
            foreach (DefaultVariable variable in defaultVariables) 
                ResetVariable(variable);
        }

        /// Set the value of a variable
        public override void SetValue(string variableName, Value value)
        {
            // Copy this value into our list
            variables[variableName] = new Value(value);
        }

        /// Get the value of a variable
        public override Value GetValue(string variableName)
        {
            // If we don't have a variable with this name, return the null value
            if (variables.ContainsKey(variableName) == false)
                return Value.NULL;
        
            return variables [variableName];
        }

        /// Erase all variables
        public override void Clear()
        {
            variables.Clear();
        }

        private void ResetVariable(DefaultVariable variable)
        {
            object value;

            switch (variable.type) 
            {
                case Value.Type.Number:
                    float.TryParse(variable.value, out float resultf);
                    value = resultf;
                    break;

                case Value.Type.String:
                    value = variable.value;
                    break;

                case Value.Type.Bool:
                    bool.TryParse(variable.value, out bool resultb);
                    value = resultb;
                    break;

                case Value.Type.Variable:
                    // We don't support assigning default variables from other variables
                    // yet
                        
                    Debug.LogErrorFormat("Can't set variable {0} to {1}: You can't " +
                                         "set a default variable to be another variable, because it " +
                                         "may not have been initialised yet.", variable.name, variable.value);
                    return;

                case Value.Type.Null:
                    value = null;
                    break;

                default:
                    throw new System.ArgumentOutOfRangeException ();
            }

            Value v = new Value(value);

            SetValue("$" + variable.name, v);
        }
    }
}