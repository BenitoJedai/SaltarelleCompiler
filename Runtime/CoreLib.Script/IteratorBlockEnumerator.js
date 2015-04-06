﻿///////////////////////////////////////////////////////////////////////////////
// IteratorBlockEnumerator

var ss_IteratorBlockEnumerator = ss.IteratorBlockEnumerator = ss.mkType(ss, 'ss.IteratorBlockEnumerator',
	function#? DEBUG IteratorBlockEnumerator$##(moveNext, getCurrent, dispose, $this) {
		this._moveNext = moveNext;
		this._getCurrent = getCurrent;
		this._dispose = dispose;
		this._this = $this;
	},
	{
		moveNext: function#? DEBUG IteratorBlockEnumerator$moveNext##() {
			try {
				return this._moveNext.call(this._this);
			}
			catch (ex) {
				if (this._dispose)
					this._dispose.call(this._this);
				throw ex;
			}
		},
		current: function#? DEBUG IteratorBlockEnumerator$current##() {
			return this._getCurrent.call(this._this);
		},
		reset: function#? DEBUG IteratorBlockEnumerator$reset##() {
			throw new ss_NotSupportedException('Reset is not supported.');
		},
		dispose: function#? DEBUG IteratorBlockEnumerator$dispose##() {
			if (this._dispose)
				this._dispose.call(this._this);
		}
	}
);

ss.initClass(ss_IteratorBlockEnumerator, null, [ss_IEnumerator, ss_IDisposable]);
