new Vue({
    el: '#app',
    data() {
        return {
            editor: null,
            //菜单折叠
            menuCollapse: false,
        }
    },
    methods: {
        initEditor() {
            var editor = editormd("editor", {
                path: "lib/editor.md/lib/"
            });
            this.editor = editor;
        },
        login() {
            var that = this
            var postData = {
                userName: that.form.username,
                password: that.form.password
            }
            $.ajax({
                url: "/account/login",
                type: "post",
                data: postData,
                success: function (res) {
                    console.log(res);
                    if (res.code == 0) {
                        window.location.href = successUrl;

                    } else {
                        alert(res.msg);
                    }
                }
            });
        }
    },
    mounted() {
        this.initEditor();
    }
})