using System;
using System.Reflection.Emit;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil.Fluent
{
	public partial class FluentEmitter
	{
		public readonly ModuleDefinition Module;

		/// <summary>
		/// Useful for Debugging.
		/// </summary>
		public string DisassembledBody => MethodDefinition.DisassembleBody(); // ncrunch: no coverage

		/// <summary>
		/// Useful for Debugging.
		/// </summary>
		public string DisassembledMethod => MethodDefinition.Disassemble(); // ncrunch: no coverage

		public readonly MethodDefinition MethodDefinition;

		public MethodBody Body => MethodDefinition.Body;

		public TypeDefinition DeclaringType
		{
			get { return MethodDefinition.DeclaringType; }
		}

		public TypeReference ReturnType
		{
			get { return MethodDefinition.ReturnType; }
		}

		public Collection<ParameterDefinition> Parameters => MethodDefinition.Parameters;

		public Collection<VariableDefinition> Variables => MethodDefinition.Body.Variables;

        public StackValidationMode StackValidationMode = Config.DefaultStackValidationMode;

		internal FluentEmitter(MethodDefinition methodDefinition)
		{
			MethodDefinition = methodDefinition;
			Module = methodDefinition.Module;
		}

	    public FluentEmitter SetStackValidationMode(StackValidationMode mode)
	    {
	        StackValidationMode = mode;
	        return this;
	    }

        public FluentEmitter WithVariable(SystemTypeOrTypeReference varType, string name = null)
        {
            var var = new VariableDefinition(varType.GetTypeReference(Module));
            if (!string.IsNullOrEmpty(name))
                var.Name = name;
            Variables.Add(var);
            return this;
        }

        public FluentEmitter WithVariable<T>(string name = null)
        {
            MethodDefinition.WithVariable(typeof(T), name);
            return this;
        }

        public FluentEmitter WithVariable(VariableDefinition var)
        {
            Variables.Add(var);
            return this;
        }
        
        /// <summary>
        /// Compilation of MethodDefinition.Body and bind to given Func or Action signature.
        /// </summary>
        public T Compile<T>()
            where T : class
        {
            return MethodDefinition.Compile<T>();
        }

	    public string DisassembleBody()
	    {
	        return MethodDefinition.DisassembleBody();
	    }

	    public DynamicMethod ToDynamicMethod()
	    {
	        return MethodDefinition.ToDynamicMethod();
	    }

	    public MethodDefinition EndEmitting()
	    {
            // todo: something to do here? e.g. stack validation ..
	        return MethodDefinition;
	    }
    }
}
