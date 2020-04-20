function getSongs() {
  $.get('http://localhost:11870/songs', function (data) {
    makeSongsList(data);
  });
}

function getComments(songId) {
  return new Promise((resolve) => {
    $.get('http://localhost:11870/comments/' + songId, function (data) {
      resolve(data);
    }).then((data) => data);
  });
}

function getRatings(songId) {
  return new Promise((resolve) => {
    $.get('http://localhost:11870/ratings/' + songId, function (data) {
      resolve(data);
    }).then((data) => data);
  });
}

async function makeSongsList(data) {
  var accordion = $('#accordion');
  for (var i = 0; i < data.length; i++) {
    var song = data[i];

    var comments = (await getComments(song.id)) || [];

    var commentsP = [];
    for (var j = 0; j < comments.length; j++) {
      var p = `<p>${comments[j].text}</p>`;

      commentsP.push(p);
    }

    var ratings = (await getRatings(song.id)) || [];

    var ratingsLI = [];
    for (var j = 0; j < ratings.length; j++) {
      var star = `<i class="fas fa-star"></i>`;

      var li = '';
      for (var k = 0; k < ratings[j].rating; k++) {
        li = li + star;
      }
      ratingsLI.push('<li>' + li + '</li>');
    }

    var collapseId = 'song' + song.id;
    var collapseHeadId = 'songHead' + song.id;

    var card = `<div class="card">
                    <div class="card-header" id="${collapseHeadId}">
                        <div class="mb-0">
                            <button class="btn btn-link" data-toggle="collapse" data-target="#${collapseId}" aria-expanded="false"
                                aria-controls="${collapseId}">
                                    <h2>${song.title} - ${song.artist}</h2>
                            </button>
                        </div>
                    </div>

                    <div id="${collapseId}" class="collapse" aria-labelledby="${collapseHeadId}" data-parent="#accordion">
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
                                            <h4>Comments</h4>
                                            <div class="comments-list">
                                                ${commentsP.join('')}
                                            </div>
                                        </header>
                                    </div>
                                    <div class="col-md-6 col-sm-12 ratings">
                                        <header>
                                            <h4>Ratings</h4>
                                            <ul class="ratings-list">
                                                ${ratingsLI.join('')}
                                            </ul>
                                        </header>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>`;

    accordion.append(card);
  }
}

getSongs();
