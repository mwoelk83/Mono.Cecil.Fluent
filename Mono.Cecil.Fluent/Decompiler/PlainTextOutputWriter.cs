// Copyright (c) 2011 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System.IO;

namespace ICSharpCode.Decompiler
{
	internal sealed class PlainTextOutput
	{
		private readonly TextWriter _writer;
		private int _indent;
		private bool _needsIndent;

		public PlainTextOutput()
		{
			_writer = new StringWriter();
		}

		public override string ToString()
		{
			return _writer.ToString();
		}

		public void Indent()
		{
			_indent++;
		}

		public void Unindent()
		{
			_indent--;
		}

		private void WriteIndent()
		{
			if (_needsIndent)
			{
				_needsIndent = false;
				for (var i = 0; i < _indent; i++)
				{
					_writer.Write('\t');
				}
			}
		}

		public void Write(char ch)
		{
			WriteIndent();
			_writer.Write(ch);
		}

		public void Write(string text)
		{
			WriteIndent();
			_writer.Write(text);
		}

		public void WriteLine()
		{
			_writer.WriteLine();
			_needsIndent = true;
		}

		public void Write(string format, params object[] args)
		{
			Write(string.Format(format, args));
		}

		public void WriteLine(string text)
		{
			Write(text);
			WriteLine();
		}

		public void WriteLine(string format, params object[] args)
		{
			WriteLine(string.Format(format, args));
		}

		public void WriteDefinition(string text, object definition, bool isLocal = true)
		{
			Write(text);
		}
	}
}
