﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Xamarin.Database;
using System;
using System.Collections.Generic;

namespace ChatAppUsingFirebase
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme ="@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity, IValueEventListener
    {
        private FirebaseClient firebaseClient;
        private List<MessageContent> lstMessage = new List<MessageContent>();
        private FirebaseAuth auth;
        private ListView lstChat;
        private EditText edtChat;
        private FloatingActionButton fab;
        public static MainActivity _mainActivity { get; set; }

        public int MyResultCode = 1;

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            auth = FirebaseAuth.Instance;

            firebaseClient = new FirebaseClient(GetString(Resource.String.firebase_database_url));
            FirebaseDatabase.Instance.GetReference("chats").AddValueEventListener(this);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            edtChat = FindViewById<EditText>(Resource.Id.input);
            lstChat = FindViewById<ListView>(Resource.Id.list_of_messages);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Firebase Chatapp";

            fab.Click += delegate { PostMessage(); };

            if (FirebaseAuth.Instance.CurrentUser == null) {
                StartActivityForResult(new Intent(this, typeof(SignIn)), MyResultCode);
            }
            else
            {
                Toast.MakeText(this, "Welcome " + FirebaseAuth.Instance.CurrentUser.Email, ToastLength.Short).Show();
                DisplayChatMessage();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_Logout:
                    LogoutUser();
                    break;
                default:
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void LogoutUser()
        {
            auth.SignOut();
            if (auth.CurrentUser == null)
            {
                Toast.MakeText(this, "Logged out!", ToastLength.Short).Show();
                StartActivityForResult(new Intent(this, typeof(SignIn)), MyResultCode);
            }
        }

        private async void PostMessage()
        {
            if (edtChat.Text == "")
            {
                Toast.MakeText(this, "Please insert a messsage to send", ToastLength.Short).Show();
            }
            else
            {
                var items = await firebaseClient.Child("chats").PostAsync(new MessageContent(FirebaseAuth.Instance.CurrentUser.Email, edtChat.Text));
                edtChat.Text = ""; 
            }
        }
        public void OnCancelled(DatabaseError error)
        {

        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            DisplayChatMessage();
        }

        private async void DisplayChatMessage()
        {
            lstMessage.Clear();
            var items = await firebaseClient.Child("chats")
                .OnceAsync<MessageContent>();
            foreach (var item in items)
                lstMessage.Add(item.Object);
            ListViewAdapter adapter = new ListViewAdapter(this, lstMessage);
            lstChat.Adapter = adapter;
        }  
    }
}

