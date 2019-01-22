using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace NPCCMobileApplications.Droid
{
    public class ScaleImageViewGestureDetector : GestureDetector.SimpleOnGestureListener
    {
        readonly ScaleImageView m_ScaleImageView;

        public ScaleImageViewGestureDetector(ScaleImageView imageView)
        {
            m_ScaleImageView = imageView;
        }

        public override bool OnDown(MotionEvent e)
        {
            return true;
        }

        public override bool OnDoubleTap(MotionEvent e)
        {
            m_ScaleImageView.MaxZoomTo((int)e.GetX(), (int)e.GetY());
            m_ScaleImageView.Cutting();
            return true;
        }
    }

    public class ScaleImageView : ImageView, View.IOnTouchListener, INotifyPropertyChanged
    {
        Matrix _matrix;
        float[] _matrixValues = new float[9];
        int _width;
        int _height;
        int _intrinsicWidth;
        int _intrinsicHeight;
        float _scale;
        float _minScale;
        float _previousDistance;
        int _previousMoveX;
        int _previousMoveY;
        bool _isScaling;
        GestureDetector _gestureDetector;

        public float MaxScale { get; set; } = 2.0f;

        bool _isScaled;
        public bool IsScaled
        {
            get
            {
                return _isScaled;
            }
            private set
            {
                _isScaled = value;
                OnPropertyChanged();
            }
        }

        public float Scale
        {
            get { return GetValue(_matrix, Matrix.MscaleX); }
        }

        public float TranslateX
        {
            get { return GetValue(_matrix, Matrix.MtransX); }
        }

        public float TranslateY
        {
            get { return GetValue(_matrix, Matrix.MtransY); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ScaleImageView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public ScaleImageView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        public override void SetImageBitmap(Bitmap bm)
        {
            base.SetImageBitmap(bm);
            Initialize();
        }

        public override void SetImageResource(int resId)
        {
            base.SetImageResource(resId);
            Initialize();
        }

        void Initialize()
        {
            SetScaleType(ScaleType.Matrix);
            _matrix = new Matrix();

            if (Drawable != null)
            {
                _intrinsicWidth = Drawable.IntrinsicWidth;
                _intrinsicHeight = Drawable.IntrinsicHeight;
                SetOnTouchListener(this);
            }

            _gestureDetector = new GestureDetector(Context, new ScaleImageViewGestureDetector(this));
        }

        protected override bool SetFrame(int l, int t, int r, int b)
        {
            _width = r - l;
            _height = b - t;

            _matrix.Reset();
            var r_norm = r - l;
            _scale = (float)r_norm / (float)_intrinsicWidth;

            var paddingHeight = 0;
            var paddingWidth = 0;
            if (_scale * _intrinsicHeight > _height)
            {
                _scale = (float)_height / (float)_intrinsicHeight;
                _matrix.PostScale(_scale, _scale);
                paddingWidth = (r - _width) / 2;
            }
            else
            {
                _matrix.PostScale(_scale, _scale);
                paddingHeight = (b - _height) / 2;
            }

            _matrix.PostTranslate(paddingWidth, paddingHeight);
            ImageMatrix = _matrix;
            _minScale = _scale;

            Cutting();

            return base.SetFrame(l, t, r, b);
        }

        float GetValue(Matrix matrix, int whichValue)
        {
            matrix.GetValues(_matrixValues);
            return _matrixValues[whichValue];
        }

        public void MaxZoomTo(int x, int y)
        {
            if (_minScale != Scale && (Scale - _minScale) > 0.1f)
            {
                var scale = _minScale / Scale;
                ZoomTo(scale, x, y);
            }
            else
            {
                var scale = MaxScale / Scale;
                ZoomTo(scale, x, y);
            }
        }

        public void ZoomTo(float scale, int x, int y)
        {
            if (Scale * scale < _minScale)
            {
                scale = _minScale / Scale;
            }
            else
            {
                if (scale >= 1 && Scale * scale > MaxScale)
                {
                    scale = MaxScale / Scale;
                }
            }
            _matrix.PostScale(scale, scale);
            //move to center
            _matrix.PostTranslate(-(_width * scale - _width) / 2, -(_height * scale - _height) / 2);

            //move x and y distance
            _matrix.PostTranslate(-(x - (_width / 2)) * scale, 0);
            _matrix.PostTranslate(0, -(y - (_height / 2)) * scale);
            ImageMatrix = _matrix;
        }

        public void Cutting()
        {
            var width = (int)(_intrinsicWidth * Scale);
            var height = (int)(_intrinsicHeight * Scale);
            if (TranslateX < -(width - _width))
            {
                _matrix.PostTranslate(-(TranslateX + width - _width), 0);
            }

            if (TranslateX > 0)
            {
                _matrix.PostTranslate(-TranslateX, 0);
            }

            if (TranslateY < -(height - _height))
            {
                _matrix.PostTranslate(0, -(TranslateY + height - _height));
            }

            if (TranslateY > 0)
            {
                _matrix.PostTranslate(0, -TranslateY);
            }

            if (width < _width)
            {
                _matrix.PostTranslate((_width - width) / 2, 0);
            }

            if (height < _height)
            {
                _matrix.PostTranslate(0, (_height - height) / 2);
            }

            ImageMatrix = _matrix;

            IsScaled = Scale > _minScale;
        }

        float Distance(float x0, float x1, float y0, float y1)
        {
            var x = x0 - x1;
            var y = y0 - y1;
            return (float)Math.Sqrt(x * x + y * y);
        }

        float DispDistance()
        {
            return (float)Math.Sqrt(_width * _width + _height * _height);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (_gestureDetector.OnTouchEvent(e))
            {
                _previousMoveX = (int)e.GetX();
                _previousMoveY = (int)e.GetY();
                return true;
            }

            var touchCount = e.PointerCount;
            switch (e.Action)
            {
                case MotionEventActions.Down:
                case MotionEventActions.Pointer1Down:
                case MotionEventActions.Pointer2Down:
                    {
                        if (touchCount >= 2)
                        {
                            var distance = Distance(e.GetX(0), e.GetX(1), e.GetY(0), e.GetY(1));
                            _previousDistance = distance;
                            _isScaling = true;
                        }
                    }
                    break;

                case MotionEventActions.Move:
                    {
                        if (touchCount >= 2 && _isScaling)
                        {
                            var distance = Distance(e.GetX(0), e.GetX(1), e.GetY(0), e.GetY(1));
                            var scale = (distance - _previousDistance) / DispDistance();
                            _previousDistance = distance;
                            scale += 1;
                            scale = scale * scale;
                            ZoomTo(scale, _width / 2, _height / 2);
                            Cutting();
                        }
                        else if (!_isScaling)
                        {
                            var distanceX = _previousMoveX - (int)e.GetX();
                            var distanceY = _previousMoveY - (int)e.GetY();
                            _previousMoveX = (int)e.GetX();
                            _previousMoveY = (int)e.GetY();

                            _matrix.PostTranslate(-distanceX, -distanceY);
                            Cutting();
                        }
                    }
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                case MotionEventActions.Pointer2Up:
                    {
                        if (touchCount <= 1)
                        {
                            _isScaling = false;
                        }
                    }
                    break;
            }
            return true;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            return OnTouchEvent(e);
        }

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}