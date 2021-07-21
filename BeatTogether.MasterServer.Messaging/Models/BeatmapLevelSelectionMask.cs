using BeatTogether.Core.Messaging.Abstractions;
using BeatTogether.MasterServer.Messaging.Enums;
using Krypton.Buffers;

namespace BeatTogether.MasterServer.Messaging.Models
{
    public class BeatmapLevelSelectionMask : IVersionedMessage
    {
        public BeatmapDifficultyMask BeatmapDifficultyMask { get; set; }
        public GameplayModifiersMask GameplayModifiersMask { get; set; }
        public ulong SongPackBloomFilterTop { get; set; }
        public ulong SongPackBloomFilterBottom { get; set; }

        public void WriteTo(ref SpanBufferWriter bufferWriter, uint protocolVersion = 4)
        {
            bufferWriter.WriteUInt8((byte)BeatmapDifficultyMask);
            if (protocolVersion < 4)
                bufferWriter.WriteUInt16((ushort)GameplayModifiersMask);
            else
                bufferWriter.WriteUInt32((uint)GameplayModifiersMask);
            bufferWriter.WriteUInt64(SongPackBloomFilterTop);
            bufferWriter.WriteUInt64(SongPackBloomFilterBottom);
        }

        public void ReadFrom(ref SpanBufferReader bufferReader, uint protocolVersion = 4)
        {
            BeatmapDifficultyMask = (BeatmapDifficultyMask)bufferReader.ReadByte();
            if (protocolVersion < 4)
                GameplayModifiersMask = (GameplayModifiersMask)bufferReader.ReadUInt16();
            else
                GameplayModifiersMask = (GameplayModifiersMask)bufferReader.ReadUInt32();
            SongPackBloomFilterTop = bufferReader.ReadUInt64();
            SongPackBloomFilterBottom = bufferReader.ReadUInt64();
        }
    }
}
