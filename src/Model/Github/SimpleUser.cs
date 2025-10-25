namespace FFDrop.Model.Github;

using System;
using System.Text.Json.Serialization;

/// <summary>
/// A GitHub user.
/// </summary>
public sealed class SimpleUser
{
    [JsonPropertyName("avatar_url")]
    public required Uri AvatarUrl { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("events_url")]
    public required string EventsUrl { get; set; }

    [JsonPropertyName("followers_url")]
    public required Uri FollowersUrl { get; set; }

    [JsonPropertyName("following_url")]
    public required string FollowingUrl { get; set; }

    [JsonPropertyName("gists_url")]
    public required string GistsUrl { get; set; }

    [JsonPropertyName("gravatar_id")]
    public required string GravatarId { get; set; }

    [JsonPropertyName("html_url")]
    public required Uri HtmlUrl { get; set; }

    [JsonPropertyName("id")]
    public required long Id { get; set; }

    [JsonPropertyName("login")]
    public required string Login { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("node_id")]
    public required string NodeId { get; set; }

    [JsonPropertyName("organizations_url")]
    public required Uri OrganizationsUrl { get; set; }

    [JsonPropertyName("received_events_url")]
    public required Uri ReceivedEventsUrl { get; set; }

    [JsonPropertyName("repos_url")]
    public required Uri ReposUrl { get; set; }

    [JsonPropertyName("site_admin")]
    public required bool SiteAdmin { get; set; }

    [JsonPropertyName("starred_at")]
    public string? StarredAt { get; set; }

    [JsonPropertyName("starred_url")]
    public required string StarredUrl { get; set; }

    [JsonPropertyName("subscriptions_url")]
    public required Uri SubscriptionsUrl { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("url")]
    public required Uri Url { get; set; }

    [JsonPropertyName("user_view_type")]
    public string? UserViewType { get; set; }
}
