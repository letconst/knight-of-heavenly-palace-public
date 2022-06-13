using System.Collections.Generic;
using UniRx;

namespace KOHP.MasterData
{
    public sealed class EditorPageManager
    {
        private RenderPage _prevPage = RenderPage.Home;

        private readonly IReadOnlyDictionary<RenderPage, EditorPage> _pages;

        private readonly System.IDisposable           _onToggleDisposable;
        private readonly ReactiveProperty<RenderPage> _currentPage = new(RenderPage.Home);

        public IReactiveProperty<RenderPage> CurrentPage => _currentPage;

        public EditorPageManager(IReadOnlyDictionary<RenderPage, EditorPage> pages)
        {
            _pages = pages;

            // ページ切り替え時のイベント登録
            _onToggleDisposable = _currentPage.Skip(1).Subscribe(OnPageToggled);
        }

        ~EditorPageManager()
        {
            // デストラクト時にSubscribeも破棄
            _onToggleDisposable.Dispose();
        }

        private void OnPageToggled(RenderPage openedPage)
        {
            // 切り替え先のページの切り替え処理実行
            _pages[openedPage]?.onToggled?.Invoke(true);

            // 切り替え前のページの切り替え処理実行
            _pages[_prevPage]?.onToggled?.Invoke(false);

            _prevPage = openedPage;
        }
    }
}
