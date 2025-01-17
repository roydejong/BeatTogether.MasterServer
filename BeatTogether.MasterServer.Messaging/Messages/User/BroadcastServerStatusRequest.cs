﻿using BeatTogether.Core.Messaging.Abstractions;
using BeatTogether.Extensions;
using BeatTogether.MasterServer.Messaging.Enums;
using BeatTogether.MasterServer.Messaging.Models;
using Krypton.Buffers;

namespace BeatTogether.MasterServer.Messaging.Messages.User
{
    public class BroadcastServerStatusRequest : IEncryptedMessage, IReliableRequest, IVersionedMessage
    {
        public uint SequenceId { get; set; }
        public uint RequestId { get; set; }
        public string ServerName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Secret { get; set; }
        public string Password { get; set; }
        public int CurrentPlayerCount { get; set; }
        public int MaximumPlayerCount { get; set; }
        public DiscoveryPolicy DiscoveryPolicy { get; set; }
        public InvitePolicy InvitePolicy { get; set; }
        public BeatmapLevelSelectionMask SelectionMask { get; set; }
        public byte[] Random { get; set; }
        public byte[] PublicKey { get; set; }
        public GameplayServerConfiguration GameplayServerConfiguration { get; set; } 

        public void WriteTo(ref SpanBufferWriter bufferWriter)
        {
            bufferWriter.WriteString(ServerName);
            bufferWriter.WriteString(UserId);
            bufferWriter.WriteString(UserName);
            bufferWriter.WriteString(Secret);
            bufferWriter.WriteString(Password);
            bufferWriter.WriteVarInt(CurrentPlayerCount);
        }

        public void WriteTo(ref SpanBufferWriter bufferWriter, uint protocolVersion)
        {
            WriteTo(ref bufferWriter);

            if (protocolVersion < 4)
            {
                bufferWriter.WriteVarInt(MaximumPlayerCount);
                bufferWriter.WriteUInt8((byte)DiscoveryPolicy);
                bufferWriter.WriteUInt8((byte)InvitePolicy);
            }
            
            SelectionMask.WriteTo(ref bufferWriter);
            
            bufferWriter.WriteBytes(Random);
            bufferWriter.WriteVarBytes(PublicKey);

            if (protocolVersion >= 4)
            {
                GameplayServerConfiguration.WriteTo(ref bufferWriter);
            }
        }

        public void ReadFrom(ref SpanBufferReader bufferReader)
        {
            ServerName = bufferReader.ReadString();
            UserId = bufferReader.ReadString();
            UserName = bufferReader.ReadString();
            Secret = bufferReader.ReadString();
            Password = bufferReader.ReadString();
            CurrentPlayerCount = bufferReader.ReadVarInt();
        }

        public void ReadFrom(ref SpanBufferReader bufferReader, uint protocolVersion)
        {
            ReadFrom(ref bufferReader);
            
            if (protocolVersion < 4)
            {
                MaximumPlayerCount = bufferReader.ReadVarInt();
                DiscoveryPolicy = (DiscoveryPolicy) bufferReader.ReadByte();
                InvitePolicy = (InvitePolicy) bufferReader.ReadByte();
            }

            SelectionMask = new BeatmapLevelSelectionMask();
            SelectionMask.ReadFrom(ref bufferReader);
            
            Random = bufferReader.ReadBytes(32).ToArray();
            PublicKey = bufferReader.ReadVarBytes().ToArray();

            if (protocolVersion >= 4)
            {
                GameplayServerConfiguration = new GameplayServerConfiguration();
                GameplayServerConfiguration.ReadFrom(ref bufferReader);

                MaximumPlayerCount = GameplayServerConfiguration.MaxPlayerCount;
                DiscoveryPolicy = GameplayServerConfiguration.DiscoveryPolicy;
                InvitePolicy = GameplayServerConfiguration.InvitePolicy;
            }
        }
    }
}
