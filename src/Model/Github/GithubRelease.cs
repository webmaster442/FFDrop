namespace FFDrop.Model.Github;

using System;
using System.Text.Json.Serialization;

/// <summary>
/// A release.
/// </summary>
public sealed class GithubRelease
{
    [JsonPropertyName("assets")]
    public required ReleaseAsset[] Assets { get; set; }

    [JsonPropertyName("assets_url")]
    public required Uri AssetsUrl { get; set; }

    /// <summary>
    /// A GitHub user.
    /// </summary>
    [JsonPropertyName("author")]
    public required AuthorClass Author { get; set; }

    [JsonPropertyName("body")]
    public string? Body { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("body_html")]
    public string? BodyHtml { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("body_text")]
    public string? BodyText { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// The URL of the release discussion.
    /// </summary>
    [JsonPropertyName("discussion_url")]
    public Uri? DiscussionUrl { get; set; }

    /// <summary>
    /// true to create a draft (unpublished) release, false to create a published one.
    /// </summary>
    [JsonPropertyName("draft")]
    public required bool Draft { get; set; }

    [JsonPropertyName("html_url")]
    public required Uri HtmlUrl { get; set; }

    [JsonPropertyName("id")]
    public required long Id { get; set; }

    /// <summary>
    /// Whether or not the release is immutable.
    /// </summary>
    [JsonPropertyName("immutable")]
    public bool? Immutable { get; set; }

    [JsonPropertyName("mentions_count")]
    public long? MentionsCount { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("node_id")]
    public required string NodeId { get; set; }

    /// <summary>
    /// Whether to identify the release as a prerelease or a full release.
    /// </summary>
    [JsonPropertyName("prerelease")]
    public required bool Prerelease { get; set; }

    [JsonPropertyName("published_at")]
    public required DateTimeOffset PublishedAt { get; set; }

    [JsonPropertyName("reactions")]
    public ReactionRollup? Reactions { get; set; }

    /// <summary>
    /// The name of the tag.
    /// </summary>
    [JsonPropertyName("tag_name")]
    public required string TagName { get; set; }

    [JsonPropertyName("tarball_url")]
    public required Uri TarballUrl { get; set; }

    /// <summary>
    /// Specifies the commitish value that determines where the Git tag is created from.
    /// </summary>
    [JsonPropertyName("target_commitish")]
    public required string TargetCommitish { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonPropertyName("upload_url")]
    public required string UploadUrl { get; set; }

    [JsonPropertyName("url")]
    public required Uri Url { get; set; }

    [JsonPropertyName("zipball_url")]
    public required Uri ZipballUrl { get; set; }
}
