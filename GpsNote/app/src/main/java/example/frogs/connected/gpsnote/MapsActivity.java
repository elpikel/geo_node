package example.frogs.connected.gpsnote;

import android.content.Intent;
import android.location.Location;
import android.os.AsyncTask;
import android.support.annotation.NonNull;
import android.support.v4.app.FragmentActivity;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.Marker;
import com.google.android.gms.maps.model.MarkerOptions;
import com.google.gson.Gson;
import com.google.gson.JsonParseException;
import com.hypertrack.lib.HyperTrack;
import com.hypertrack.lib.callbacks.HyperTrackCallback;
import com.hypertrack.lib.models.ErrorResponse;
import com.hypertrack.lib.models.SuccessResponse;
import com.hypertrack.lib.models.User;
import com.squareup.okhttp.Call;
import com.squareup.okhttp.MediaType;
import com.squareup.okhttp.OkHttpClient;
import com.squareup.okhttp.Request;
import com.squareup.okhttp.RequestBody;
import com.squareup.okhttp.Response;

import java.io.IOException;
import java.util.UUID;

public class MapsActivity extends AppCompatActivity implements OnMapReadyCallback {

    private void GoToPage(String id)
    {
        Intent intent = new Intent(this, NotesActivity.class);
        intent.putExtra("placeId", Integer.parseInt(id));
        intent.putExtra("userName", user.getName());
        startActivity(intent);
    }

    User user;

    private class GetPlaces extends AsyncTask<Void, Void, Place[]> {

        private Call createCall() {
            OkHttpClient client = new OkHttpClient();

            Request request = new Request.Builder()
                    .url("https://young-thicket-35712.herokuapp.com/places")
                    .build();

            return client.newCall(request);
        }

        private Place[] convertToPlaces(Response response) throws IOException {
            String stringPlaces = response.body().string();

            try {
                return new Gson().fromJson(stringPlaces, Place[].class);
            } catch (JsonParseException e) {
                return new Place[0];
            }
        }

        protected Place[] doInBackground(Void... nothing) {

            Call getPlacesCall = createCall();

            try {
                Response response = getPlacesCall.execute();

                if(response.code() == 400) {
                    return new Place[0];
                }

                return convertToPlaces(response);
            } catch (IOException e) {
                e.printStackTrace();
            }

            return new Place[0];
        }

        protected void onPostExecute(Place[] places) {
            // bind places to view

            for(int i = 0; i < places.length; i++)
            {
                Place place = places[i];
                // Add a marker in Sydney and move the camera
                LatLng sydney = new LatLng(place.latitude, place.longitude);
                MarkerOptions title = new MarkerOptions()
                        .position(sydney)
                        .title(place.description);


                Marker marker = mMap.addMarker(title);
                marker.setTag(place.id);

                mMap.moveCamera(CameraUpdateFactory.newLatLng(sydney));
            }



        }
    }


    private class SaveNoteWithoutPlace extends AsyncTask<Void, Note, Note> {
        public final MediaType JSON = MediaType.parse("application/json; charset=utf-8");

        private NoteWithoutPlace note;

        public SaveNoteWithoutPlace(NoteWithoutPlace note) {
            this.note = note;
        }

        @Override
        protected Note doInBackground(Void... params) {
            try {
                Call registerCall = createCall();

                Response response = registerCall.execute();

                return convert(response);

            } catch (IOException e) {
                e.printStackTrace();
            }

            return null;
        }

        protected void onPostExecute(Note note) {
            // bind notes to view

            int id = note.id;
        }

        private Call createCall() throws IOException {
            OkHttpClient client = new OkHttpClient();

            Request request = new Request.Builder()
                    .url("https://young-thicket-35712.herokuapp.com/notes")
                    .post(createRequestBody())
                    .build();

            return client.newCall(request);
        }

        private RequestBody createRequestBody() throws IOException {
            Gson gson = new Gson();
            try {
                return RequestBody.create(JSON, gson.toJson(new NoteNoteWithoutPlacePostBody(this.note)));
            } catch (Exception e) {
                throw new IOException("problem creating json from note");
            }
        }

        private Note convert(Response response) throws IOException {
            String jsonMessages = response.body().string();

            return new Gson().fromJson(jsonMessages, Note.class);
        }
    }

    private GoogleMap mMap;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_maps);
        // Obtain the SupportMapFragment and get notified when the map is ready to be used.
        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.map);
        mapFragment.getMapAsync(this);
        Button button = (Button) this.findViewById(R.id.button);

        HyperTrack.initialize(this, "pk_935da9b6e1d113608f98d6de03d5079c8d791c01 ");


        if (!HyperTrack.checkLocationPermission(this)) {
            HyperTrack.requestPermissions(this);
            return;
        }

        // Check for Location settings
        if (!HyperTrack.checkLocationServices(this)) {
            HyperTrack.requestLocationServices(this, null);
        }

        HyperTrack.createUser(UUID.randomUUID().toString(), "000000000", new HyperTrackCallback() {
            @Override
            public void onSuccess(@NonNull SuccessResponse successResponse) {
                // Hide Login Button loader
                //loginBtnLoader.setVisibility(View.GONE);

                 user = (User) successResponse.getResponseObject();

                // Handle createUser success here, if required
                // HyperTrack SDK auto-configures UserId on createUser API call, so no need to call
                // HyperTrack.setUserId() API

                // On UserLogin success
//                onUserLoginSuccess();
            }

            @Override
            public void onError(@NonNull ErrorResponse errorResponse) {
                // Hide Login Button loader


            }
        });


        //HyperTrack.initialize(this, "pk_935da9b6e1d113608f98d6de03d5079c8d791c01 ");

        final EditText messageForm = (EditText) this.findViewById(R.id.message);

        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                HyperTrack.getCurrentLocation(new HyperTrackCallback() {
                    @Override
                    public void onSuccess(@NonNull SuccessResponse successResponse) {
                        Location responseObject = (Location) successResponse.getResponseObject();
                        double altitude = responseObject.getLatitude();
                        double longitude = responseObject.getLongitude();

                        NoteWithoutPlace noteWithoutPlace = new NoteWithoutPlace();
                        noteWithoutPlace.description = messageForm.getText().toString();
                        noteWithoutPlace.longitude = (float)longitude;
                        noteWithoutPlace.latitude = (float)altitude;
                        noteWithoutPlace.user_name = user.getName();


                        (new SaveNoteWithoutPlace(noteWithoutPlace)).execute();
                        messageForm.setText("");
                    }

                    @Override
                    public void onError(@NonNull ErrorResponse errorResponse) {

                    }
                });

//                noteWithoutPlace.
//                new SaveNoteWithoutPlace()
            }
        });
    }


    /**
     * Manipulates the map once available.
     * This callback is triggered when the map is ready to be used.
     * This is where we can add markers or lines, add listeners or move the camera. In this case,
     * we just add a marker near Sydney, Australia.
     * If Google Play services is not installed on the device, the user will be prompted to install
     * it inside the SupportMapFragment. This method will only be triggered once the user has
     * installed Google Play services and returned to the app.
     */
    @Override
    public void onMapReady(GoogleMap googleMap) {

        mMap = googleMap;

        googleMap.setOnMarkerClickListener(new GoogleMap.OnMarkerClickListener() {
            @Override
            public boolean onMarkerClick(Marker marker) {

                GoToPage(marker.getTag().toString());
                return true;
            }
        });
        (new GetPlaces()).execute();
    }




}
