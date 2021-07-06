if redis.call("EXISTS", @serverKey) == 1 then
    redis.call("HSET", @serverKey, "CurrentPlayerCount", @currentPlayerCount)
end
if tonumber(@isQuickPlay) == 1 then
    redis.call("ZADD", @publicServersByPlayerCountKey, @currentPlayerCount, @secret)
end
