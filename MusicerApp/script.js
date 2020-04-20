var baseUrl = 'http://localhost:11870/';
var loadingPlaceholder = 'Loading song...';

function getSongs() {
  $.get(baseUrl + 'songs', function (data) {
    makeSongsList(data);
  });
}

function getComments(songId) {
  return new Promise((resolve) => {
    $.get(baseUrl + 'comments/' + songId, function (data) {
      resolve(data);
    }).then((data) => data);
  });
}

function getRatings(songId) {
  return new Promise((resolve) => {
    $.get(baseUrl + 'ratings/' + songId, function (data) {
      resolve(data);
    }).then((data) => data);
  });
}

function loadSongFile(songId, idForAudioContainer) {
  var container = $('#' + idForAudioContainer)[0];
  if (container.innerText.trim() == loadingPlaceholder) {
    var audioTag = `<audio id="player-${songId}" controls>
                        <source src="${baseUrl}songs/${songId}/listen" 
                                type="audio/mp3" />
                        Your browser does not support the audio tag.
                    </audio>`;

    container.innerHTML = audioTag;
  }
  // pause all the rest
  $('audio').map((k, v) => v.pause());
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

    var songCard = `<div class="card">
                    <div class="card-header" id="${collapseHeadId}">
                        <div class="mb-0">
                            <button onClick="loadSongFile(${song.id}, 
                                            'song-audio-${song.id}')"
                                class="btn btn-link" data-toggle="collapse" data-target="#${collapseId}" aria-expanded="false"
                                aria-controls="${collapseId}">
                        <h2>${song.title} - ${song.artist} | ${song.genre}</h2>
                            </button>
                        </div>
                    </div>

                    <div id="${collapseId}" class="collapse" aria-labelledby="${collapseHeadId}" data-parent="#accordion">
                        <div class="card-body">
                            <div class="container">
                            <div class="row">
                                <div id="song-audio-${song.id}"
                                >${loadingPlaceholder}</div>
                            </div>
                            <hr>                                
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
                    
                        <div class="container">
                            <div class="row">
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">New comment</span>
                                    </div>
                                    <textarea class="form-control"
                                        id="new-comment-${song.id}"></textarea>
                                    <button class="btn btn-primary comment-btn"
                                            onClick="addComment(${song.id}, 
                                                    'new-comment-${song.id}')">
                                        Add Comment
                                    </button>
                                </div>
                            </div>
                            <div class="row rating-row">
                                <p>New rating</p>
                                <div class="rating-wrapper">
                                    <input type="radio" value="5" class="rating-input" onChange="currentRating(5)"
                                            id="rating-input-${song.id}-5" 
                                            name="rating${song.id}" />
                                    <label for="rating-input-${song.id}-5" 
                                            class="rating-star"></label>                                    
                                    <input type="radio" value="4" class="rating-input" onChange="currentRating(4)"
                                            id="rating-input-${song.id}-4" 
                                            name="rating${song.id}" />
                                    <label for="rating-input-${song.id}-4" 
                                            class="rating-star"></label>
                                    <input type="radio" value="3" class="rating-input" onChange="currentRating(3)"
                                            id="rating-input-${song.id}-3" 
                                            name="rating${song.id}" />
                                    <label for="rating-input-${song.id}-3" 
                                            class="rating-star"></label>
                                    <input type="radio" value="2" class="rating-input" onChange="currentRating(2)"
                                            id="rating-input-${song.id}-2" 
                                            name="rating${song.id}" />
                                    <label for="rating-input-${song.id}-2" 
                                            class="rating-star"></label>
                                    <input type="radio" value="1" class="rating-input" onChange="currentRating(1)"
                                            id="rating-input-${song.id}-1" 
                                            name="rating${song.id}" />
                                    <label for="rating-input-${song.id}-1" 
                                            class="rating-star"></label>
                                </div>
                                <button class="btn btn-primary" 
                                        onClick="addRating(${song.id})">
                                        Add Rating
                                </button>
                            </div>
                        </div>
                    </div>
                </div>`;

    accordion.append(songCard);
  }

  $('#loadMsg').hide(800);
}

async function addComment(songId, textareaId) {
  var text = $('#' + textareaId)[0].value.trim();
  if (text.length < 20) {
    alert('No valid comment, at least 20 chars!');
  } else {
    await postComment(text, songId);
    refresh();
  }
}

function postComment(Text, SongId) {
  var obj = JSON.stringify({ SongId, Text });
  return new Promise((resolve) => {
    $.ajax({
      type: 'POST',
      url: baseUrl + 'comments',
      contentType: 'application/json',
      data: obj,
      dataType: 'json',
      success: function (data) {
        resolve(data);
      },
    });
  }).then((data) => data);
}

var rating = 0;
function currentRating(r) {
  rating = r;
}

async function addRating(songId) {
  if (rating > 0 && rating < 6) {
    await postRating(rating, songId);
    rating = 0;
    refresh();
  } else {
    alert('No valid rating!');
  }
}

function postRating(Rating, SongId) {
  var obj = JSON.stringify({ SongId, Rating });
  return new Promise((resolve) => {
    $.ajax({
      type: 'POST',
      url: baseUrl + 'ratings',
      contentType: 'application/json',
      data: obj,
      dataType: 'json',
      success: function (data) {
        resolve(data);
      },
    });
  }).then((data) => data);
}

function refresh() {
  $('#accordion')[0].innerHTML = '';
  getSongs();
}

// ------------------ song upload ---------------------------

var fileName;
$('.custom-file-input').on('change', function () {
  fileName = $(this).val();
  if (fileName.endsWith('.mp3') == false) {
    alert('Not mp3 file!');
    $(this).val('');
    return;
  }

  fn = $(this).val().split('\\').pop();
  $(this).siblings('.custom-file-label').addClass('selected').html(fn);
});

$('#uploadBtn').on('click', async function (e) {
  e.preventDefault();

  if ($('#fileinfo')[0].checkValidity() == false) {
    $('#fileinfo')[0].reportValidity();
    return;
  }

  if (!fileName || fileName.length == 0) {
    alert('Please, pick mp3 file.');
    return;
  } else {
    $('#fileinfo').hide();
    $('#uploadMsg').show();
    var song = await createSong(
      $('#song-title').val().trim(),
      $('#song-artist').val().trim(),
      $('#song-genre').val().trim().toLowerCase()
    );
    if (await uploadSong(song.id)) {
      alert('Success!');
      window.location = 'index.html';
    } else {
      alert('FAILED!');
    }
  }
});

function createSong(title, artist, genre) {
  return new Promise((resolve) => {
    var objData = JSON.stringify({ title, artist, genre });
    $.ajax({
      url: baseUrl + 'songs',
      type: 'POST',
      data: objData,
      processData: true,
      contentType: 'application/json',
      success: function (data) {
        resolve(data);
      },
    });
  }).then((data) => data);
}

function uploadSong(id) {
  return new Promise((resolve) => {
    var fd = new FormData();
    fd.append('file', $('#customFile')[0].files[0]);
    $.ajax({
      url: baseUrl + 'songs/' + id + '/uploadFile',
      type: 'POST',
      data: fd,
      cache: false,
      contentType: false,
      processData: false,
      success: function (data) {
        resolve(data);
      },
    });
  }).then((data) => data);
}
