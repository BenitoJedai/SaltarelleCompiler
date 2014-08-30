﻿using NUnit.Framework;

namespace Saltarelle.Compiler.Tests.CompilerTests.MethodCompilation.Statements {
	[TestFixture]
	public class ExpressionStatementTests : MethodCompilerTestBase {
		[Test]
		public void ExpressionStatementThatOnlyRequiresASingleScriptStatementWorks() {
			AssertCorrect(
@"public void M() {
	int i;
	// BEGIN
	i = 0;
	// END
}",
@"	$i = 0;
");
		}

		[Test]
		public void ExpressionStatementThatRequiresMultipleScriptStatementsWorks() {
			AssertCorrect(
@"public int P1 { get; set; }
public int P2 { get; set; }
public int P3 { get; set; }
public void M() {
	int i;
	// BEGIN
	i = (P1 = P2 = P3 = 1);
	// END
}",
@"	this.set_$P3(1);
	this.set_$P2(1);
	this.set_$P1(1);
	$i = 1;
");
		}

		[Test]
		public void CallToPartialMethodWithoutDefinitionIsRemoved() {
			AssertCorrect(
@"partial class C {
	partial void Method();

	public void M() {
		// BEGIN
		int x = 0;
		Method();
		int y = 0;
		// END
	}
}",
@"	var $x = 0;
	var $y = 0;
", addSkeleton: false);
		}

		[Test]
		public void CallToExternMethodIsNotRemoved() {
			AssertCorrect(
@"class C {
	extern void Method();

	public void M() {
		// BEGIN
		int x = 0;
		Method();
		int y = 0;
		// END
	}
}",
@"	var $x = 0;
	this.$Method();
	var $y = 0;
", addSkeleton: false);
		}

		[Test]
		public void CallToPartialMethodWithDefinitionIsNotRemoved() {
			AssertCorrect(
@"partial class C {
	partial void Method() {
	}
}
partial class C {
	partial void Method();

	public void M() {
		// BEGIN
		int x = 0;
		Method();
		int y = 0;
		// END
	}
}",
@"	var $x = 0;
	this.$Method();
	var $y = 0;
", addSkeleton: false);
		}

		[Test]
		public void CallToConditionalMethodIsRemovedWhenTheSymbolIsNotDefined() {
			AssertCorrect(
@"class C {
	[System.Diagnostics.Conditional(""__TEST__"")]
	void Method() {
	}

	public void M() {
		// BEGIN
		int x = 0;
		Method();
		int y = 0;
		// END
	}
}",
@"	var $x = 0;
	var $y = 0;
", addSkeleton: false);
		}

		[Test]
		public void CallToConditionalMethodIsNotRemovedWhenTheSymbolIsDefined() {
			AssertCorrect(
@"
#define __TEST__
class C {
	[System.Diagnostics.Conditional(""__TEST__"")]
	void Method() {
	}
	public void M() {
		// BEGIN
		int x = 0;
		Method();
		int y = 0;
		// END
	}
}",
@"	var $x = 0;
	this.$Method();
	var $y = 0;
", addSkeleton: false);
		}

		[Test]
		public void UndefCausesCallToConditionalMethodToBeRemoved() {
			AssertCorrect(
@"
#define __TEST__
#undef __TEST__
class C {
	[System.Diagnostics.Conditional(""__TEST__"")]
	void Method() {
	}

	public void M() {
		// BEGIN
		int x = 0;
		Method();
		int y = 0;
		// END
	}
}",
@"	var $x = 0;
	var $y = 0;
", addSkeleton: false);
		}
	}
}
