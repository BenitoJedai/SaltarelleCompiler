﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Xebic.Parsers.ES3;
using Saltarelle.Compiler.JSModel.Expressions;
using Saltarelle.Compiler.JSModel.Statements;

namespace JavaScriptParser {
	public static class Parser {
		public static JsExpression ParseExpression(string source, bool singleLineCommentsAreStatements = false) {
			var lex = new ES3Lexer(new ANTLRStringStream(source)) { SingleLineCommentsAreStatements = singleLineCommentsAreStatements };
			var tokens = new CommonTokenStream(lex);
			var parser = new ES3Parser(tokens);

			var r = parser.expressionOnly();
			var tree = new ES3Walker(new CommonTreeNodeStream(r.Tree));
			return tree.expression();
		}

		public static JsStatement ParseStatement(string source, bool singleLineCommentsAreStatements = false) {
			var lex = new ES3Lexer(new ANTLRStringStream(source.Trim())) { SingleLineCommentsAreStatements = singleLineCommentsAreStatements };
			var tokens = new CommonTokenStream(lex);
			var parser = new ES3Parser(tokens);

			var r = parser.sourceElement();
			var tree = new ES3Walker(new CommonTreeNodeStream(r.Tree));
			return tree.statement();
		}

		public static IList<JsStatement> ParseProgram(string source, bool singleLineCommentsAreStatements = false) {
			var lex = new ES3Lexer(new ANTLRStringStream(source)) { SingleLineCommentsAreStatements = singleLineCommentsAreStatements };
   			var tokens = new CommonTokenStream(lex);
			var parser = new ES3Parser(tokens);

			var r = parser.program();
			var tree = new ES3Walker(new CommonTreeNodeStream(r.Tree));
			return tree.program();
		}
	}
}
