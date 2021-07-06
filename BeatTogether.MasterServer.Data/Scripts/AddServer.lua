if redis.call("EXISTS", @serverKey) == 1 then
    return false
end
redis.call(
    "HSET", @serverKey,
    "HostUserId", @hostUserId,
    "HostUserName", @hostUserName,
    "RemoteEndPoint", @remoteEndPoint,
    "Code", @code,
    "IsQuickPlay", @isQuickPlay,
    "DiscoveryPolicy", @discoveryPolicy,
    "InvitePolicy", @invitePolicy,
    "BeatmapDifficultyMask", @beatmapDifficultyMask,
    "GameplayModifiersMask", @gameplayModifiersMask,
    "SongPackBloomFilterTop", @songPackBloomFilterTop,
    "SongPackBloomFilterBottom", @songPackBloomFilterBottom,
    "CurrentPlayerCount", @currentPlayerCount,
    "MaximumPlayerCount", @maximumPlayerCount,
    "Random", @random,
    "PublicKey", @publicKey
)
redis.call("HSET", @serversByCodeKey, @code, @secret)
if tonumber(@isQuickPlay) == 1 then
    redis.call("ZADD", @publicServersByPlayerCountKey, @currentPlayerCount, @secret)
end
return true
