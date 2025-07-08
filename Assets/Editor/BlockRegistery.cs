using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent.Editor
{
    [CreateAssetMenu(fileName = nameof(BlockRegistery), menuName = "OrbAscent/" + nameof(BlockRegistery))]
    public class BlockRegistery : ScriptableObject
    {
        [Serializable]
        public struct BlockData
        {
            public GameObject blockPrefab;
            public BlockType blockType;
        }

        private Dictionary<BlockType, GameObject> _blockRegistery;
        private bool _registryInitialized = false;

        public List<BlockData> blocksData;

        private void Init()
        {
            _blockRegistery = new Dictionary<BlockType, GameObject>();
            foreach (BlockData blockData in blocksData)
            {
                _blockRegistery[blockData.blockType] = blockData.blockPrefab;
            }
            _registryInitialized = true;
        }

        private void OnDisable()
        {
            _registryInitialized = false;
        }

        public GameObject GetBlock(BlockType blockType)
        {
            if (!_registryInitialized)
            {
                Init();
            }

            if (_blockRegistery.ContainsKey(blockType))
            {
                return _blockRegistery[blockType];
            }

            return null;
        }
    }
}
