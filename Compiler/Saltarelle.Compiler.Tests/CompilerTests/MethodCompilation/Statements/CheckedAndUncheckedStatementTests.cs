﻿using NUnit.Framework;

namespace Saltarelle.Compiler.Tests.CompilerTests.MethodCompilation.Statements {
	[TestFixture]
	public class CheckedAndUncheckedStatementTests : MethodCompilerTestBase {
		[Test]
		public void CheckedStatementActsAsABlockStatement() {
			AssertCorrect(
@"public void M() {
	// BEGIN
	checked {
		int x = 0;
	}
	// END
}",
@"	{
		// @(4, 3) - (4, 13)
		var $x = 0;
	}
", addSourceLocations: true);
		}

		[Test]
		public void UncheckedStatementActsAsABlockStatement() {
			AssertCorrect(
@"public void M() {
	// BEGIN
	unchecked {
		int x = 0;
	}
	// END
}",
@"	{
		// @(4, 3) - (4, 13)
		var $x = 0;
	}
", addSourceLocations: true);
		}
	}
}
