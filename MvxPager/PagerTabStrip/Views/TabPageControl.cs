using System;
using UIKit;
using CoreGraphics;
using MvxPagerTabStrip.Models;

namespace MvxPagerTabStrip.Views
{
	public class TabPageControl : UIControl
	{	
		#region Fields

		nint _currentPage;
		nint _numberOfPages;

		UIImage _dotImage;
		TabDotShape _dotShape = TabDotShape.Circle;
		nfloat _dotSize;
		UIColor _dotColor;
		UIColor _dotShadowColor;
		nfloat _dotShadowBlur;
		CGSize _dotShadowOffset;

		UIImage _selectedDotImage;
		TabDotShape _selectedDotShape = TabDotShape.Circle;
		nfloat _selectedDotSize;
		UIColor _selectedDotColor;
		UIColor _selectedDotShadowColor;
		nfloat _selectedDotShadowBlur;
		CGSize _selectedDotShadowOffset;

		nfloat _dotSpacing;

		bool _disposed = false;

		#endregion

		#region Constructors

		public TabPageControl ()
		{
			SetUp ();
		}

		public TabPageControl (CGRect frame) : base(frame)
		{
			SetUp ();
		}

		public TabPageControl (Foundation.NSCoder coder) : base (coder)
		{
			SetUp ();
		}

		#endregion

		#region Properties

		public virtual bool DefersCurrentPageDisplay { get; set; }
		public virtual bool HidesForSinglePage { get; set; }
		public virtual bool IsWrappedEnabled { get; set; }
		public virtual bool Vertical { get; set; }

		public override CGSize IntrinsicContentSize {
			get {
				return SizeThatFits (Bounds.Size);
			}
		}

		public virtual TabDotShape DotShape {
			get { return _dotShape;	}
			set {
				_dotShape = value;
				SetNeedsDisplay ();
			}
		}

		public virtual UIImage DotImage {
			get { return _dotImage;	}
			set {
				_dotImage = value;
				SetNeedsDisplay ();
			}
		}

		public virtual nfloat DotSize {
			get { return _dotSize;	}
			set {
				if (NMath.Abs(_dotSize - value) > 0.001) {
					_dotSize = value;
					SetNeedsDisplay ();					
				}
			}
		}

		public virtual UIColor DotColor {
			get { return _dotColor;	}
			set {
				if (_dotColor != value) {
					_dotColor = value;
					SetNeedsDisplay ();					
				}
			}
		}

		public virtual UIColor DotShadow {
			get { return _dotShadowColor;	}
			set {
				if (_dotColor != value && _dotShadowColor != value) {
					_dotShadowColor = value;
					SetNeedsDisplay ();					
				}
			}
		}

		public virtual nfloat DotShadowBlur {
			get { return _dotShadowBlur;	}
			set {
				if (NMath.Abs(_dotShadowBlur - value) > 0.001) {
					_dotShadowBlur = value;
					SetNeedsDisplay ();					
				}
			}
		}

		public virtual CGSize DotShadowOffset {
			get { return _dotShadowOffset;	}
			set {
				if (_dotShadowOffset != value) {
					_dotShadowOffset = value;
					SetNeedsDisplay ();					
				}
			}
		}

		public virtual TabDotShape SelectedDotShape {
			get { return _selectedDotShape;	}
			set {
				_selectedDotShape = value;
				SetNeedsDisplay ();
			}
		}

		public virtual UIImage SelectedDotImage {
			get { return _selectedDotImage;	}
			set {
				_selectedDotImage = value;
				SetNeedsDisplay ();
			}
		}

		public virtual nfloat SelectedDotSize {
			get { return _selectedDotSize;	}
			set {
				if (NMath.Abs(_selectedDotSize - value) > 0.001) {
					_selectedDotSize = value;
					SetNeedsDisplay ();					
				}
			}
		}

		public virtual UIColor SelectedDotColor {
			get { return _selectedDotColor;	}
			set {
				if (_selectedDotColor != value) {
					_selectedDotColor = value;
					SetNeedsDisplay ();					
				}
			}
		}

		public virtual UIColor SelectedDotShadow {
			get { return _selectedDotShadowColor;	}
			set {
				if (_selectedDotColor != value && _selectedDotShadowColor != value) {
					_selectedDotShadowColor = value;
					SetNeedsDisplay ();					
				}
			}
		}

		public virtual nfloat SelectedDotShadowBlur {
			get { return _selectedDotShadowBlur;	}
			set {
				if (NMath.Abs(_selectedDotShadowBlur - value) > 0.001) {
					_selectedDotShadowBlur = value;
					SetNeedsDisplay ();					
				}
			}
		}

		public virtual CGSize SelectedDotShadowOffset {
			get { return _selectedDotShadowOffset;	}
			set {
				if (_selectedDotShadowOffset != value) {
					_selectedDotShadowOffset = value;
					SetNeedsDisplay ();					
				}
			}
		}

		public virtual nfloat DotSpacing
		{
			get { return _dotSpacing; }
			set {
				if (NMath.Abs(_dotSpacing - value) > 0.001) {
					_dotSpacing = value;
					SetNeedsDisplay ();
				}
			}
		}

		public virtual nint CurrentPage {
			get { return _currentPage;	}
			set {
				_currentPage = ClampedPageValue (value);
				SetNeedsDisplay ();
			}
		}

		public virtual nint NumberOfPages {
			get { return _numberOfPages;	}
			set {
				if (_numberOfPages != value) {
					_numberOfPages = value;
					if (_currentPage >= value) {
						_currentPage = value - 1;
					}
					SetNeedsDisplay ();
				}
			}
		}

		#endregion

		#region Methods

		protected void SetUp ()
		{	
			//needs redrawing if bounds change
			ContentMode = UIViewContentMode.Redraw;
			BackgroundColor = UIColor.Clear;
			//set defaults
			_dotSpacing = 5f;
			_dotSize = 6.0f;
			_selectedDotSize = _dotSize;
			_dotShadowOffset = new CGSize (0, 1);
			_selectedDotShadowOffset = new CGSize(0, 1);
			_dotColor = UIColor.Black;
			_selectedDotColor = UIColor.White;
		}


		public CGSize SizeForNumbaerOfPager()
		{
			nfloat width = _dotSize + (_dotSize + _dotSpacing) * (_numberOfPages - 1);
			return Vertical ? new CGSize (_dotSize, width) : new CGSize (width, _dotSize);
		}

		protected void UpdatedCurrentPageDisplay ()
		{
			SetNeedsDisplay ();
		}

		public override void Draw (CGRect rect)
		{
			if (_numberOfPages > 1 || !HidesForSinglePage) {
				using (var context = UIGraphics.GetCurrentContext ()) {
					CGSize size = SizeForNumbaerOfPager ();
					context.SetAllowsAntialiasing (true);
					if (Vertical) {
						context.TranslateCTM (Frame.Size.Width / 2, (Frame.Size.Height - size.Height) / 2);
					} else {
						context.TranslateCTM ((Frame.Size.Width - size.Width) / 2, Frame.Size.Height / 2);
					}
					for (int i = 0; i < _numberOfPages; i++) {
						UIImage dotImage = null;
						UIColor dotColor = null;
						TabDotShape dotShape = TabDotShape.Circle;
						UIColor dotShadowColor = null;
						CGSize dotShaddowOffset = CGSize.Empty;
						nfloat dotShalodwBlur = 0;
						nfloat dotSize = 0;

						if (i == _currentPage) {
//							_selectedDotColor.SetFill ();
							dotImage = _selectedDotImage;
							dotShape = _selectedDotShape;
							dotColor = _selectedDotColor;
							dotShalodwBlur = _selectedDotShadowBlur;
							dotShadowColor = _selectedDotShadowColor;
							dotShaddowOffset = _selectedDotShadowOffset;
							dotSize = _selectedDotSize;
						} else {
//							_dotColor.SetFill ();
							dotImage = _dotImage;
							dotShape = _dotShape;
							dotColor = _dotColor.ColorWithAlpha (0.25f);
							dotShalodwBlur = _dotShadowBlur;
							dotShadowColor = _dotShadowColor;
							dotShaddowOffset = _dotShadowOffset;
							dotSize = _dotSize;
						}
						context.SaveState ();
						nfloat offset = (_dotSize + _dotSpacing) * i + _dotSize / 2;
						context.TranslateCTM (Vertical ? 0 : offset, Vertical ? offset : 0);
						if (dotShadowColor != null && dotShadowColor != UIColor.Clear) {
							context.SetShadow (dotShaddowOffset, dotShalodwBlur, dotShadowColor.CGColor);
						}
						if (dotImage != null) {
							dotImage.Draw (new CGRect (-dotImage.Size.Width / 2, -dotImage.Size.Height / 2, dotImage.Size.Width, dotImage.Size.Height));
						} else {
							dotColor.SetFill ();
							if (dotShape == TabDotShape.Circle) {
								context.FillEllipseInRect (new CGRect (-dotSize / 2, -dotSize / 2, dotSize, dotSize));
							} else if (dotShape == TabDotShape.Square) {
								context.FillRect (new CGRect (-dotSize / 2, -dotSize / 2, dotSize, dotSize));
							} else if (true) {
								context.BeginPath ();
								context.MoveTo (0, -dotSize / 2);
								context.MoveTo (dotSize / 2, dotSize / 2);
								context.MoveTo (-dotSize / 2, dotSize / 2);
								context.MoveTo (0, -dotSize / 2);
								context.FillPath ();
							}
						}
						context.RestoreState ();
					}
				}
			}
		}

		public virtual nint ClampedPageValue(nint page)
		{
			if (IsWrappedEnabled) {
				return _numberOfPages != 0 ? (page + _numberOfPages) % _numberOfPages : 0;
			} else {
				return NMath.Min (NMath.Max (0, page), _numberOfPages - 1);
			}
		}


		public override void EndTracking (UITouch uitouch, UIEvent uievent)
		{
			CGPoint point = uitouch.LocationInView (this);
			bool forward = Vertical ? (point.Y > Frame.Size.Height / 2) : (point.X > Frame.Size.Width / 2);
			_currentPage = ClampedPageValue (_currentPage + (forward ? 1 : -1));
			if (!DefersCurrentPageDisplay) {
				SetNeedsDisplay ();
			}
			SendActionForControlEvents (UIControlEvent.ValueChanged);
			base.EndTracking (uitouch, uievent);
		}

		public override CGSize SizeThatFits (CGSize size)
		{
			CGSize dotSize = SizeForNumbaerOfPager ();
			if (_selectedDotSize != 0) {
				nfloat width = (_selectedDotSize - _dotSize);
				nfloat height = NMath.Max (36, NMath.Max (_dotSize, _selectedDotSize));
				dotSize.Width = Vertical ? height : dotSize.Width + width;
				dotSize.Height = Vertical ? dotSize.Height + width : height;
			}
			if ((_dotShadowColor != null && _dotShadowColor != UIColor.Clear) ||
				(_selectedDotShadowColor != null && _selectedDotShadowColor != UIColor.Clear)) 
			{
				dotSize.Width += NMath.Max(_dotShadowOffset.Width, _selectedDotShadowOffset.Width) * 2;
				dotSize.Height += NMath.Max(_dotShadowOffset.Height, _selectedDotShadowOffset.Height) * 2;
				dotSize.Width += NMath.Max(_dotShadowBlur, _selectedDotShadowBlur) * 2;
				dotSize.Height += NMath.Max (_dotShadowBlur, _selectedDotShadowBlur) * 2;
			}
			return dotSize;
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			if (!_disposed) {
				_dotImage = null;
				_selectedDotImage = null;
				_disposed = true;
			}
		}

		#endregion
	}
}