package example.frogs.connected.gpsnote;

import android.os.AsyncTask;

import com.google.gson.Gson;
import com.google.gson.JsonParseException;
import com.squareup.okhttp.Call;
import com.squareup.okhttp.OkHttpClient;
import com.squareup.okhttp.Request;
import com.squareup.okhttp.Response;

import java.io.IOException;

public class GetPlaces extends AsyncTask<Void, Void, Place[]> {

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

    }
}

