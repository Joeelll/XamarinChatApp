using Android.App;
using Android.Gms.Tasks;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Firebase;
using Firebase.Auth;

namespace ChatAppUsingFirebase
{
    [Activity(Label = "SignIn", Theme ="@style/Theme.AppCompat.Light.NoActionBar")]
    public class SignIn : AppCompatActivity, IOnCompleteListener
    {
        FirebaseAuth auth;

        public void OnComplete(Task task)
        {
            if (task.IsComplete)
            {
                Toast.MakeText(this, "Sign In Successful !", ToastLength.Short).Show();
                Finish();
            }
            else
            {
                Toast.MakeText(this, "Sing In field !", ToastLength.Short).Show();
                Finish();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignIn);
            auth = FirebaseAuth.Instance;
            var edtEmail = FindViewById<EditText>(Resource.Id.edtEmail);
            var edtPassword = FindViewById<EditText>(Resource.Id.edtPassword);
            var btnSignIn = FindViewById<Button>(Resource.Id.btnSingIn);
            var btnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            btnSignIn.Click += delegate 
            {
                if (edtEmail.Text == ""  && edtPassword.Text == "" || edtEmail.Text == "" || edtPassword.Text == "")
                {
                    Toast.MakeText(Application.Context, "Please enter a valid email and password", ToastLength.Short).Show();
                }
                else
                {
                    auth.SignInWithEmailAndPassword(edtEmail.Text, edtPassword.Text)
                    .AddOnCompleteListener(this);
                }
            };

            btnSignUp.Click += delegate
            {
                if (edtEmail.Text == "" && edtPassword.Text == "" || edtEmail.Text == "" || edtPassword.Text == "")
                {
                    Toast.MakeText(Application.Context, "Please enter a valid email and password", ToastLength.Short).Show();
                }
                else
                {
                    auth.CreateUserWithEmailAndPassword(edtEmail.Text, edtPassword.Text)
                    .AddOnCompleteListener(this);
                }
            };
        }
    }
}