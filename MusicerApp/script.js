function getSongs() {
  $.get('http://localhost:11870/songs', function (data) {
    makeSongsList(JSON.parse(data));
  });
}

function getComments(songId) {
  $.get('http://localhost:11870/comments/' + songId, function (data) {
    return JSON.parse(data);
  });
}

function getRatings(songId) {
  $.get('http://localhost:11870/ratings/' + songId, function (data) {
    return JSON.parse(data);
  });
}

function makeSongsList(data) {
  var accordion = $('#accordion');
  for (var i = 0; i < data.length; i++) {
    var song = data[i];

    var comments = getComments(song.id);

    var commentsP = [];
    for (var j = 0; j < comments.length; j++) {
      var p = `<p>${comments[j].text}</p>`;

      commentsP.push(p);
    }

    var ratings = getRatings(song.id);

    var ratingsIL = [];
    for (var j = 0; j < ratings.length; j++) {
      var il = `<il>${ratings[j].text}</il>`;

      ratingsIL.push(p);
    }

    var card = `<div class="card">
                    <div class="card-header" id="headingOne">
                        <h5 class="mb-0">
                            <button class="btn btn-link" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true"
                                aria-controls="collapseOne">
                                    ${song.title} - ${song.artist}
                            </button>
                        </h5>
                    </div>

                    <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordion">
                        <div class="card-body">
                            <div class="container">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <p>${song.genre}</p>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 col-sm-12 comments">
                                        <header>
                                            <h3>Comments</h3>
                                            <div class="comments-list">
                                                ${commentsP}
                                            </div>
                                        </header>
                                    </div>
                                    <div class="col-md-6 col-sm-12 ratings">
                                        <header>
                                            <h3>Ratings</h3>
                                            <ul class="ratings-list">
                                                ${ratingsIL}
                                            </ul>
                                        </header>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>`;

    accordion.appendChild(card);
  }
}

$(document).ready(function () {
  getSongs();
});
