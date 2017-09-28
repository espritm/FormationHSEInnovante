using Android.Support.V7.App;
using Android.App;
using Android.Views;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using QuelleFormationSecurite.Droid.Utils;
using QuelleFormationSecurite.Droid.Fragments;
using Android.Content;
using QuelleFormationSecurite.Droid.Activities;
using Android.Runtime;

namespace QuelleFormationSecurite.Droid
{
	[Activity (Icon = "@drawable/icon")]
	public class Activity_Accueil : AppCompatActivity
    { 
        DrawerLayout m_drawerLayout;
        NavigationView m_navigationView;
        Fragment_List_Results m_fragmentAccueil;
        Fragment_LegalTerms m_fragmentLegalTerms;
        Fragment_CGU m_fragmentCGU;
        FragmentEnum m_currentFragment;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            
			SetContentView (Resource.Layout.Activity_Accueil);
            m_drawerLayout = FindViewById<DrawerLayout>(Resource.Id.ActivityAccueil_drawerlayout);
            m_navigationView = FindViewById<NavigationView>(Resource.Id.ActivityAccueil_navigationView);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu_white_24dp);

            m_navigationView.InflateMenu(Resource.Menu.menu_navigationView_accueil);
            m_navigationView.NavigationItemSelected += M_navigationView_NavigationItemSelected;
            ConfigureNavigationViewHeader();
            
            GoToFragment(FragmentEnum.Accueil);
        }

        public void GoToFragment(FragmentEnum position)
        {
            switch (position)
            {
                case FragmentEnum.Accueil:
                    if (m_fragmentAccueil == null)
                        m_fragmentAccueil = new Fragment_List_Results();

                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.ActivityAccueil_frameLayout_viewForFragments, m_fragmentAccueil, ((int)FragmentEnum.Accueil).ToString()).CommitAllowingStateLoss();

                    SupportActionBar.Title = Resources.GetString(Resource.String.menuNavigationViewAccueil_accueil);
                    break;

                case FragmentEnum.Questions:
                    Intent intent = new Intent(this, typeof(Activity_Questions));
                    StartActivityForResult(intent, 42);
                    break;

                case FragmentEnum.LegalTerms:
                    if (m_fragmentLegalTerms == null)
                        m_fragmentLegalTerms = new Fragment_LegalTerms();

                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.ActivityAccueil_frameLayout_viewForFragments, m_fragmentLegalTerms, ((int)FragmentEnum.LegalTerms).ToString()).CommitAllowingStateLoss();

                    SupportActionBar.Title = Resources.GetString(Resource.String.menuNavigationViewAccueil_legal);
                    break;

                case FragmentEnum.CGU:
                    if (m_fragmentCGU == null)
                        m_fragmentCGU = new Fragment_CGU();

                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.ActivityAccueil_frameLayout_viewForFragments, m_fragmentCGU, ((int)FragmentEnum.CGU).ToString()).CommitAllowingStateLoss();

                    SupportActionBar.Title = Resources.GetString(Resource.String.menuNavigationViewAccueil_cgu);
                    break;
            }

            m_currentFragment = position;
            InvalidateOptionsMenu();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            outState.PutInt("iCurrentFragment", (int)m_currentFragment);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);

            m_currentFragment = (FragmentEnum)savedInstanceState.GetInt("iCurrentFragment");

            GoToFragment(m_currentFragment);
        }

        private void ConfigureNavigationViewHeader()
        {
            View viewHeader = LayoutInflater.Inflate(Resource.Layout.NavigationviewHeader, null);

            m_navigationView.AddHeaderView(viewHeader);
        }

        private void M_navigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case Resource.Id.menuNavigationViewAccueil_accueil:
                    GoToFragment(FragmentEnum.Accueil);
                    break;

                case Resource.Id.menuNavigationViewAccueil_start:
                    GoToFragment(FragmentEnum.Questions);
                    break;

                case Resource.Id.menuNavigationViewAccueil_legal:
                    GoToFragment(FragmentEnum.LegalTerms);
                    break;

                case Resource.Id.menuNavigationViewAccueil_cgu:
                    GoToFragment(FragmentEnum.CGU);
                    break;
            }
            m_drawerLayout.CloseDrawer(GravityCompat.Start);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_accueil, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    //If menu is open, close it. Else, open it.
                    if (m_drawerLayout.IsDrawerOpen(GravityCompat.Start))
                        m_drawerLayout.CloseDrawers();
                    else
                        m_drawerLayout.OpenDrawer(GravityCompat.Start);
                    break;

                case Resource.Id.menu_accueil_removeResults:
                    Helpers.Settings.lsJsonResults = "";
                    m_fragmentAccueil.RefreshTestResultList();
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == 42)
                m_fragmentAccueil.RefreshTestResultList();

            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}


